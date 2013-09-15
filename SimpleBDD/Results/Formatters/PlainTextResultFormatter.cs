using System.Text;

namespace SimpleBDD.Results.Formatters
{
	/// <summary>
	/// Formats feature results as plain text.
	/// </summary>
	public class PlainTextResultFormatter : IResultFormatter
	{
		#region IResultFormatter Members

		/// <summary>
		/// Formats feature results.
		/// </summary>
		/// <param name="features">Features to format.</param>
		public string Format(params IFeatureResult[] features)
		{
			var builder = new StringBuilder();
			foreach (var feature in features)
				FormatFeature(builder, feature);
			return builder.ToString();
		}

		private void FormatFeature(StringBuilder builder, IFeatureResult feature)
		{
			foreach (var scenario in feature.Scenarios)
				FormatScenario(builder, scenario);
		}

		#endregion

		private void FormatScenario(StringBuilder builder, IScenarioResult scenario)
		{
			if (builder.Length > 0)
				builder.AppendLine();
			builder.Append("Scenario: ").Append(scenario.Name).Append(" - ").AppendLine(scenario.Status.ToString());
			foreach (var step in scenario.Steps)
			{
				builder.Append("\tStep ")
					   .Append(step.Number).Append(": ")
					   .Append(step.Name).Append(" - ").AppendLine(step.Status.ToString());
			}
		}
	}
}