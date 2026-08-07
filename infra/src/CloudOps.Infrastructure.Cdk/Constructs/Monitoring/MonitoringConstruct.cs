using Amazon.CDK;
using Amazon.CDK.AWS.CloudWatch;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.CloudWatch.Actions;
using Constructs;
using CloudOps.Infrastructure.Cdk.Common;
using CloudOps.Infrastructure.Cdk.Configuration;
using CloudOps.Infrastructure.Cdk.Models;

namespace CloudOps.Infrastructure.Cdk.Constructs.Monitoring;

public sealed class MonitoringConstruct : Construct
{
    private const string MetricNamespace = "CloudOps/EventPlatform";
    public MonitoringResources Resources { get; }

    public MonitoringConstruct(Construct scope, string id, PlatformConfiguration config, Topic alertsTopic): base(scope, id)
    {
        var dashboard = new Dashboard(this, "Dashboard", new DashboardProps
        {
            DashboardName = ResourceNaming.DashboardName(config)
        });

        var eventsAccepted = CreateMetric("EventsAccepted");

        var eventsProcessed = CreateMetric("EventsProcessed");

        var eventsPersisted = CreateMetric("EventsPersisted");

        var eventsFailed = CreateMetric("EventsFailed");

        var duplicateEvents = CreateMetric("DuplicateEvents");

        dashboard.AddWidgets(

            new GraphWidget(new GraphWidgetProps
            {
                Title = "Events Accepted",

                Left = [eventsAccepted]
            }),

            new GraphWidget(new GraphWidgetProps
            {
                Title = "Events Processed",

                Left = [eventsProcessed]
            }),

            new GraphWidget(new GraphWidgetProps
            {
                Title = "Events Persisted",

                Left = [eventsPersisted]
            }),

            new GraphWidget(new GraphWidgetProps
            {
                Title = "Processing Failures",

                Left = [eventsFailed]
            }),

            new GraphWidget(new GraphWidgetProps
            {
                Title = "Duplicate Events",

                Left = [duplicateEvents]
            })
        );

        var failedAlarm = new Alarm(this, "EventsFailedAlarm", new AlarmProps
        {
            AlarmName = $"{config.ApplicationName}-{config.Environment}-events-failed",

            AlarmDescription = "Raised whenever event processing failures occur.",

            Metric = eventsFailed,

            Threshold = 1,

            EvaluationPeriods = 1,

            DatapointsToAlarm = 1,

            ComparisonOperator = ComparisonOperator.GREATER_THAN_OR_EQUAL_TO_THRESHOLD
        });

        failedAlarm.AddAlarmAction(new SnsAction(alertsTopic));

        var duplicateAlarm = new Alarm(this, "DuplicateEventsAlarm", new AlarmProps
        {
            AlarmName = $"{config.ApplicationName}-{config.Environment}-duplicate-events",

            AlarmDescription = "Raised when duplicate events are detected.",

            Metric = duplicateEvents,

            Threshold = 5,

            EvaluationPeriods = 1,

            DatapointsToAlarm = 1,

            ComparisonOperator = ComparisonOperator.GREATER_THAN_OR_EQUAL_TO_THRESHOLD
        });

        duplicateAlarm.AddAlarmAction(new SnsAction(alertsTopic));

        new Amazon.CDK.CfnOutput(this,"DashboardName", new Amazon.CDK.CfnOutputProps
        {
            Value = dashboard.DashboardName
        });

        Resources = new MonitoringResources(dashboard, failedAlarm, duplicateAlarm);
    }

    private static Metric CreateMetric(string metricName)
    {
        return new Metric(new MetricProps
        {
            Namespace = MetricNamespace,

            MetricName = metricName,

            Statistic = "Sum",

            Period = Duration.Minutes(5)
        });
    }
}