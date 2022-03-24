# Sort-visualizer
Avalonia project to show the differences between sorting algorithms

## Technologies used
- [Microsoft .NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Avalonia](https://avaloniaui.net/)

## Usage
First you need to clone this repository, please make:
``` 
git clone https://github.com/Mzzay/sort-visualizer.git && cd sort-visualizer
```

Verify that you have dotnet 6.0 installed, after this please make :
``` 
dotnet run
```

After this you can choose between random array or custom one. 

🛈 If you're using custom array, you must separate value by `;`. 

## Executables files
* For windows:
```
dotnet publish --output "./output_windows" --runtime win-x64 --configuration Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
```

* For linux:
``` 
dotnet publish --output "./output_linux" --runtime linux-x64 --configuration Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
``` 

## Contacts:
For any suggestion or remark, you can contact me via [Discord](https://discord.com/users/498432113666818058).  
