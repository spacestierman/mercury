using Mercury.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	static class MercuryProjectParser
	{
		public static MercuryProject TryToParseFile(string absoluteFilePath)
		{
			string contents = File.ReadAllText(absoluteFilePath);
			return TryToParseContents(contents);
		}

		public static MercuryProject TryToParseContents(string fileContents)
		{
			MercuryProject project = Newtonsoft.Json.JsonConvert.DeserializeObject<MercuryProject>(fileContents);
			project.Settings = ParseSettings(project._settings);

			foreach (MercuryPluginJson pluginData in project._plugins)
			{
				MercuryPlugin plugin = (MercuryPlugin)TryToBuildObjectFromTypeString(pluginData.Type);
				if (plugin == null)
				{
					throw new FormattedException("Unable to determine the plugin type \"{0}\".", pluginData.Type);
				}

				plugin.Settings.Merge(project.Settings);
				foreach (KeyValuePair<string, object> setting in pluginData.Settings)
				{
					plugin.Settings.Set(setting.Key, setting.Value);
				}

				project.Plugins.Add(plugin);
			}

			project.Entities = ParseEntities(project._entities);

			return project;
		}

		private static object TryToBuildObjectFromTypeString(string typeName)
		{
			Type type = Type.GetType(typeName);
			object instance = Activator.CreateInstance(type);
			return instance;
		}

		private static MercurySettings ParseSettings(Dictionary<string, object> set)
		{
			MercurySettings settings = new MercurySettings();
			if (set != null)
			{
				foreach (KeyValuePair<string, object> setting in set)
				{
					settings.Set(setting.Key, setting.Value);
				}
			}

			return settings;
		}

		private static IEnumerable<MercuryEntity> ParseEntities(IEnumerable<MercuryEntityData> set)
		{
			List<MercuryEntity> list = new List<MercuryEntity>();
			foreach (MercuryEntityData entityJson in set)
			{
				MercuryEntity entity = new MercuryEntity();
				entity.Name = entityJson.Name;
				entity.DisplayName = entityJson.DisplayName;
				entity.Fields = ParseEntityFields(entityJson.Fields);

				if (entity.HasMultiplePrimaryKeys)
				{
					throw new FormattedException("We do not support multiple primary keys yet.");
				}

				list.Add(entity);
			}

			HookupEntityReferences(list, set);

			return list;
		}

		private static IEnumerable<MercuryField> ParseEntityFields(IEnumerable<MercuryEntityFieldJson> fields)
		{
			List<MercuryField> list = new List<MercuryField>();

			foreach (MercuryEntityFieldJson json in fields)
			{
				MercuryField field = ParseEntityFields(json);
				list.Add(field);
			}

			list.Last().IsLastField = true;

			return list;
		}

		private static MercuryField ParseEntityFields(MercuryEntityFieldJson json)
		{
			MercuryField field = new MercuryField();
			field.Name = json.Name;
			field.DisplayName = json.DisplayName;
			field.IsPrimaryKey = json.IsPrimaryKey;
			field.Type = ParseFieldType(json.Type);
			field.UiHint = TryToParseUiHint(json.UiHint);
			field.ValidationRules = ParseValidationRules(json.Validation);

			return field;
		}

		private static MercuryFieldType ParseFieldType(string field)
		{
			if (string.IsNullOrEmpty(field))
			{
				throw new ArgumentRequiredException("field");
			}

			switch (field.ToLower())
			{
				case "integer":
					return MercuryFieldType.INTEGER;
				case "nvarchar":
				case "varchar":
					return MercuryFieldType.VARCHAR;
				case "text":
					return MercuryFieldType.TEXT;
				default:
					throw new FormattedException("Unable to determine field type for \"{0}\".", field);
			}
		}

		private static MercuryUiHint? TryToParseUiHint(string hint)
		{
			if (string.IsNullOrEmpty(hint))
			{
				return null;
			}

			switch (hint.ToLower())
			{
				case "textbox":
					return MercuryUiHint.Textbox;
				case "textarea":
					return MercuryUiHint.Textarea;
				case "checkbox":
					return MercuryUiHint.Checkbox;
				case "radio":
					return MercuryUiHint.Radio;
				case "dropdown":
					return MercuryUiHint.DropDown;
				default:
					return null;
			}
		}

		private static IEnumerable<MercuryValidationRule> ParseValidationRules(IEnumerable<MercuryValidationRuleJson> list)
		{
			List<MercuryValidationRule> rules = new List<MercuryValidationRule>();

			foreach (MercuryValidationRuleJson json in list)
			{
				MercuryValidationRule rule = ParseValidationRule(json);
				rules.Add(rule);
			}

			return rules;
		}

		private static MercuryValidationRule ParseValidationRule(MercuryValidationRuleJson json)
		{
			MercuryValidationRule rule = TryToBuildObjectFromTypeString(json.Type) as MercuryValidationRule;
			if (rule == null)
			{
				throw new FormattedException("Unable to build a validation rule of type \"{0}\".", json.Type);
			}

			MercurySettings settings = ParseSettings(json._settings);
			rule.LoadFromSettings(settings);

			return rule;
		}

		private static void HookupEntityReferences(IEnumerable<MercuryEntity> entities, IEnumerable<MercuryEntityData> json)
		{
			foreach (MercuryEntity entity in entities)
			{
				MercuryEntityData entityJson = json.First(x => x.Name == entity.Name);
				foreach (MercuryField field in entity.Fields)
				{
					MercuryEntityFieldJson fieldJson = entityJson.Fields.First(x => x.Name == field.Name);
					if (fieldJson.HasReference)
					{
						MercuryEntity referencedEntity = entities.First(x => x.Name == fieldJson.Reference.Entity);
						MercuryField displayField = referencedEntity.Fields.First(x => x.Name == fieldJson.Reference.Display);

						MercuryFieldReference reference = new MercuryFieldReference(referencedEntity, displayField);
						field.Reference = reference;
					}
				}
			}
		}
	}
}
