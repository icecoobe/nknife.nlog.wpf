[nuget]: https://www.nuget.org/packages/NKnife.NLog.Wpf/
![NuGet](https://img.shields.io/nuget/v/nlogviewer.svg)

NKnife.NLog.Wpf
==========

NKnife.NLog.Wpf is a simple WPF-control to show NLog-logs. 

## How to use?

Add a namespace to your Window, like this:

        xmlns:nlog ="clr-namespace:NlogViewer;assembly=NlogViewer"

then add the control.

        <nlog:NlogViewer x:Name="logCtrl" /> 

To setup NlogViewer as a target, add the following to your Nlog.config.

```xml
  <extensions>
    <add assembly="NlogViewer" />
  </extensions>
  <targets>
    <target xsi:type="NlogViewer" name="ctrl" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="ctrl" />
  </rules>
```

## Nuget

A NuGet-package is available [here][nuget]. It will try to install the control and a sample Nlog.config.
