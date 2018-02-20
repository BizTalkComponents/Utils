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


## Message helpers ##
IBaseMessage extension methods for reading and manipulating the message using XPath.

```c#
//Select one value
string xPath = "/root/element1[1]";
string result = msg.Select(xPath);

//Select multiple values
string xPath1 = "/root/element1[1]";
string xPath2 = "/root/element2[1]";
Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

//Replace one or multiple values based on a single XPath
string xPath = "/root/element[1]";
string newValue = "newValue";
msg.Replace(xPath, newValue);

//Replace multiple values based on multiple XPaths
string xPath1 = "/root/element1[1]";
string xPath2 = "/root/element2[1]";
string newValue1 = "newValue1";
string newValue2 = "newValue2";

var replacements = new Dictionary<string, string>();
replacements.Add(xPath1, newValue1);
replacements.Add(xPath2, newValue2);

msg.ReplaceMultiple(replacements);

//Find and replace one or multiple values based on a single XPath
string xPath = "/root/element";
string find = "value";
string newValue = "newValue";

msg.FindReplace(xPath, newValue, find);

//Find and replace multiple values based on multiple XPaths
string xPath1 = "/root/element1[1]";
string find1 = "value1";
string newValue1 = "newValue";
string xPath2 = "/root/element2[1]";
string find2 = "value2";
string newValue2 = "newValue";

var replacements = new Dictionary<string, KeyValuePair<string, string>>();
var findReplace1 = new KeyValuePair<string, string>(find1, newValue1);
var findReplace2 = new KeyValuePair<string, string>(find2, newValue2);
replacements.Add(xPath1, findReplace1);
replacements.Add(xPath2, findReplace2);

msg.FindReplaceMultiple(replacements);

//Do custom transformations based on multiple XPaths
string xPath = "/root/element[1]";

Func<string, string> increment = x =>
{
    int y = int.Parse(x);
    y++;
    return y.ToString();
};

var xPathToMutatorMap = new Dictionary<string, Func<string, string>>();
xPathToMutatorMap.Add(xPath, increment);

msg.Mutate(xPathToMutatorMap);
```


