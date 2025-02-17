﻿using System.Diagnostics;
using System.Reflection;

namespace ChurchManager.Domain.Shared
{
    public static class DomainConstants
    {
        public static class Groups
        {
            public const int NoParentGroupId = 0;
        }

        public static class Discipleship
        {
            public static class StepStatus
            {

                public const string NotStarted = "Not Started";
                public const string InProgress = "In Progress";
                public const string Completed = "Completed";
            }

        }

        public static class Communication
        {
            public static class Email
            {
                public static string TemplatePath => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Path.Combine("Email", "Templates"));
                public static string TemplateExtension = ".liquid";

                public static string Template(string name) => Path.Combine(TemplatePath, $"{name}{TemplateExtension}");

                // Templates
                public static class Templates
                {
                    public static string FollowUpTemplate = "FollowUpAssignment";
                }
            }
        }

        public static class Telemetry
        {
            public static readonly ActivitySource ActivitySource = new("ChurchManager.Application");
        }
    }
}
