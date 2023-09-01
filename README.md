# MetricsDelta
This little CLI (Command-line interface) tool is capable of comparing two code metric reports and generates single report that contains delta (difference) changes as well as gradings on metrics.
Metrics are graded according to configurable thresholds. This is useful in monitoring changes in quality/maintainability of code.
MetricsDelta can be used as part of pipeline quality checks.

More information about code metrics report and how to generate it can be found here:

https://learn.microsoft.com/en-us/visualstudio/code-quality/how-to-generate-code-metrics-data

Also here is some additional reading on how to interpret code metrics report values:

https://learn.microsoft.com/en-us/visualstudio/code-quality/code-metrics-values


## Installation 
### From Github
Manually download latest release and unpack it to desired location.

### From nuget.org
Use following command:

	nuget install MetricsDelta

## Usage
    MetricsDelta.exe --previousMetricsFilePath <path> --currentMetricsFilePath <path> --reportFilePath <path> --settingsFilePath <path>

### Arguments
* ``--previousMetricsFilePath`` - (Mandatory) Path to code metrics report which was generated before current code metrics report
* ``--currentMetricsFilePath`` - (Mandatory) Path to current (later than previous) code metrics report
* ``--reportFilePath`` - (Mandatory) Path to output code metrics report which will contain additional metrics delta and grading information
* ``--settingsFilePath`` - (Optional) Path to JSON settings file which could contain various overrides i.e. specific thresholds or logging options

## Known limitations
* Generated output report is stripped from any *Namespace* nodes which were part of *Assembly* nodes. This limitation will be removed in upcomming updates.

## Development information

### Source code language
**C#**

### Platform
**.NET 7** for now

### DevEnv
**MSVS 2022** (Community or compatible)





