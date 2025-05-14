# 🛠️ Testing the Result pattern to ensure correct behavior.
1. Clone this repository:
```git clone https://github.com/faulycoelho/result-pattern.git"```
```cd result-pattern\"```
2. Run tests
```dotnet test --collect:"Xplat Code Coverage"```

3. HTML report (optional)
```reportgenerator -reports:"WebApplicationResultPattern.Tests\TestResults\{**GUID_FROM_PREVIUS_STEP**}\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html```

Example:
![image](https://github.com/user-attachments/assets/458326b8-6cb7-4739-9fad-189f506b9a5e)


report HTML:
![image](https://github.com/user-attachments/assets/2ef44fda-019f-4164-ad0c-2ceeb739f5d5)
