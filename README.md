[![Build status](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Utils?branch=master)](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/Utilsy/branch/master)

BizTalkComponents.Utils is a library of helper methods intended to reduce the amount of code needed to develop custom pipeline components.

[Validation](#validation)  
[Context helpers](#contexthelpers)  
[Propertybag helpers](#propertybaghelpers)  

<a name="validation"/>

## Validation ##
Validation attribute for validating properties at runtime. Unlike the Required attribute the RequiredRuntime won't give a compilation error at design time.

```c#
//Will not give compilation error if missing but will throw an exception if it is missing at runtime.
[RequiredRuntime]
public string DateFormat { get; set; }
```
The validation should be triggered in the pipeline components Execute method using the libraries Validate method that will check all annotated properties for null.

```c#
public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
{
    string errorMessage;

    if (!Validate(out errorMessage))
    {
        throw new ArgumentException(errorMessage);
    }

   // ...
}
```
<a name="contexthelpers"/>

## Context helpers ##
Entity class representing a context property.

```c#
//Initialize from context string.
var property = new ContextProperty("http://tempuri.org#MyProp");

//Get namespace and property name
var propertyName = property.PropertyName;
var propertyNamespace = property.PropertyNamespace;

//Get context string
var contextString = property.ToPropertyString();
```

Extension methods for the standard IBaseMessageContext to support the ContextProperty entity on standard context operations.

```c#
//Promote a property using the new ContextProperty entity.
ctx.Promote(prop, "value");

//Read a property using the new ContextProperty entity.
var val = ctx.Read(prop);
```

The utils library also contains an extension method for TryRead. TryRead works like TryRead and TryParse in standard .NET and returns a boolean indicating if the operation was successful.

```c#
if (!ctx.TryRead(prop, out val))
{
	throw new InvalidOperationException("Could not find the specified property in BizTalk context.");
}
```

The TryRead method also has generic support to return a strongly typed context property value.

```c#
string val;

if(!ctx.TryRead<string>(property, out val))
{
	throw new InvalidOperationException("Could not find the specified property in BizTalk context.");
}
```
## Property bag helpers ##

<a name="propertybaghelpers"/>
BizTalkComponents.Utils has a generic version of the ReadPropertyBag and WritePropertyBag methods that are typically included in all parameterized pipeline components.

```c#
bool b;
b = PropertyBagHelper.ReadPropertyBag<bool>(propertyBag, PromoteProperytName);
```

The standard WritePropertyBag is also included so that it doesn't have to be added to every single pipeline component.

```c#
PropertyBagHelper.WritePropertyBag(propertyBag, PropertyName, PropertyValue);
```

There is also methods for reading and writing all properties of the current pipeline component.
```c#
PropertyBagHelper.WriteAll(propertyBag, this);
PropertyBagHelper.ReadAll(propertyBag, this);
```

