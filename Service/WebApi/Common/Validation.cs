using System;

namespace WebApi.Common.Validation
{
	public class ValidationIssue
    {
        public string Name { get; set; }
        public string Property { get; set; }
        public string Message { get; set; }

        public ValidationIssue(string name, string property, string message)
		{
            this.Name = name;
            this.Property = property;
            this.Message = message;
		}
	}

    public class ValidationIssues
    {
        public List<ValidationIssue> Warnings { get; set; }
        public List<ValidationIssue> Errors { get; set; }

        public ValidationIssues()
        {
            this.Warnings = new List<ValidationIssue>();
            this.Errors = new List<ValidationIssue>();
        }
    }
}

