# dotnet-base24  [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/nikeee/dotnet-base24/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/Base24)](https://www.nuget.org/packages/Base24)

A [base24](https://www.kuon.ch/post/2020-02-27-base24/) implementation for .NET Standard 2.1. It encodes binary data to human-readable text and reduces human-made errors when transmitting verbally.

## Installation
```shell
dotnet add package Base24
```

## Usage
```csharp
var data = new byte[] { 1, 2, 3, 4 };

string encodedString = Base24Encoding.Encode(data);
Console.WriteLine(encodedString);

var decodedData = Base24Encoding.Decode(encodedString);
```
