using System;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Extent.ExtentHtmlReporter.Activities.Properties;

namespace Extent.ExtentHtmlReporter.Activities
{
    class CheckParentConstraint
    {
        private static string ValidationMessage(string parentTypeName)
        {
            return string.Format(Resources.ParentScope_ValidationMessage, (object)parentTypeName);
        }

        public static Constraint GetCheckParentConstraint<ActivityType>(
        string parentTypeName,
        string validationMessage = null)
        where ActivityType : Activity
        {
            validationMessage = validationMessage ?? CheckParentConstraint.ValidationMessage(parentTypeName);
            DelegateInArgument<ValidationContext> delegateInArgument1 = new DelegateInArgument<ValidationContext>();
            DelegateInArgument<ActivityType> delegateInArgument2 = new DelegateInArgument<ActivityType>();
            DelegateInArgument<Activity> parent = new DelegateInArgument<Activity>();
            Variable<bool> variable1 = new Variable<bool>();
            Variable<IEnumerable<Activity>> variable2 = new Variable<IEnumerable<Activity>>();
            Constraint<ActivityType> constraint1 = new Constraint<ActivityType>();
            Constraint<ActivityType> constraint2 = constraint1;
            ActivityAction<ActivityType, ValidationContext> activityAction1 = new ActivityAction<ActivityType, ValidationContext>();
            activityAction1.Argument1 = delegateInArgument2;
            activityAction1.Argument2 = delegateInArgument1;
            ActivityAction<ActivityType, ValidationContext> activityAction2 = activityAction1;
            Sequence sequence1 = new Sequence();
            sequence1.Variables.Add((Variable)variable1);
            sequence1.Variables.Add((Variable)variable2);
            sequence1.Activities.Add((Activity)new Assign<IEnumerable<Activity>>()
            {
                To = (OutArgument<IEnumerable<Activity>>)((Variable)variable2),
                Value = (InArgument<IEnumerable<Activity>>)((Activity<IEnumerable<Activity>>)new GetParentChain()
                {
                    ValidationContext = (InArgument<ValidationContext>)((DelegateArgument)delegateInArgument1)
                })
            });
            Collection<Activity> activities = sequence1.Activities;
            ForEach<Activity> forEach1 = new ForEach<Activity>();
            forEach1.Values = (InArgument<IEnumerable<Activity>>)((Variable)variable2);
            ForEach<Activity> forEach2 = forEach1;
            ActivityAction<Activity> activityAction3 = new ActivityAction<Activity>();
            activityAction3.Argument = parent;
            ActivityAction<Activity> activityAction4 = activityAction3;
            If if1 = new If();
            if1.Condition = new InArgument<bool>((Expression<Func<ActivityContext, bool>>)(ctx => parent.Get(ctx).GetType().Name.Equals(parentTypeName)));
            if1.Then = (Activity)new Assign<bool>()
            {
                Value = (InArgument<bool>)true,
                To = (OutArgument<bool>)((Variable)variable1)
            };
            If if2 = if1;
            activityAction4.Handler = (Activity)if2;
            ActivityAction<Activity> activityAction5 = activityAction3;
            forEach2.Body = activityAction5;
            ForEach<Activity> forEach3 = forEach1;
            activities.Add((Activity)forEach3);
            sequence1.Activities.Add((Activity)new AssertValidation()
            {
                Assertion = new InArgument<bool>((Variable)variable1),
                Message = new InArgument<string>(validationMessage)
            });
            Sequence sequence2 = sequence1;
            activityAction2.Handler = (Activity)sequence2;
            ActivityAction<ActivityType, ValidationContext> activityAction6 = activityAction1;
            constraint2.Body = activityAction6;
            return (Constraint)constraint1;
        }
    }
}
