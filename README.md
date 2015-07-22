A set of utility classes to help out when developing pipeline components.

##Validation
New attribute for validating properties at runtime. Unlike the Required attribute the RequiredRuntime won't give a compilation error at design time.

```
//Will not give compilation error if missing but will throw an exception if it is missing at runtime.
[RequiredRuntime]
public string DateFormat { get; set; }
```
##Context helpers
Entity class representing a context property

```
//Initialize from context string.
var property = new ContextProperty("http://tempuri.org#MyProp");
//Get namespace and property name
var propertyName = property.PropertyName;
var propertyNamespace = property.PropertyNamespace;
//Get context string
var contextString = property.ToPropertyString();
```

Extension methods for supporting the ContextProperty entity on standard context operations.

```
ctx.Promote(prop, "value");
var val = ctx.Read(prop);
```

The utils library also contains an extension method for TryRead. TryRead works like TryRead and TryParse in standard .NET and returns a boolean indicating if the operation was successful.

```
if (!ctx.TryRead(prop, out val))
{
	throw new InvalidOperationException("Could not find the specified property in BizTalk context.");
}
```

The TryRead method also has generic support to return a strongly typed context property value.

```
 TryRead<string>(ctx, property, out val);
```

[![Build status](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Utils?branch=master)](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Utilsy/branch/master)
