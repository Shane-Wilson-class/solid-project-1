# solid-project-1

[![.NET CI/CD Pipeline](https://github.com/oop-jccc/solid-project-1/actions/workflows/ci.yml/badge.svg)](https://github.com/oop-jccc/solid-project-1/actions/workflows/ci.yml)

This repository contains a .NET application with a fully configured development environment for optimal productivity.

## Quick Start

### Option 1: GitHub Codespaces (Recommended)
The fastest way to get started is using GitHub Codespaces, which provides a fully configured development environment in the cloud.

1. Click the **Code** button on this repository
2. Select **Codespaces** tab
3. Click **Create codespace on master**
4. Wait for the environment to initialize (this may take a few minutes)
5. Once ready, you can immediately start coding with full IntelliSense and debugging support

### Option 2: Local Development with VS Code
1. **Prerequisites:**
   - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [Visual Studio Code](https://code.visualstudio.com/)
   - [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

2. **Clone and Setup:**
   ```bash
   git clone https://github.com/USER/REPO.git
   cd REPO
   code .
   ```

3. **Restore Dependencies:**
   ```bash
   dotnet restore solid-project-1\solid-project-1.csproj
   ```

## Build and Debug

### Using VS Code Tasks
This repository includes pre-configured VS Code tasks for common operations:

- **Build:** `Ctrl+Shift+P` → "Tasks: Run Task" → "build"
- **Run:** `Ctrl+Shift+P` → "Tasks: Run Task" → "run"
- **Clean:** `Ctrl+Shift+P` → "Tasks: Run Task" → "clean"
- **Watch:** `Ctrl+Shift+P` → "Tasks: Run Task" → "watch" (auto-rebuilds on file changes)

### Using Command Line
```bash
# Navigate to the project directory
cd solid-project-1

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Clean build artifacts
dotnet clean

# Watch for changes and auto-rebuild
dotnet watch run
```

### Debugging in VS Code
1. Open the project in VS Code
2. Set breakpoints by clicking in the left margin of the code editor
3. Press `F5` or go to **Run and Debug** panel
4. Select ".NET Core Launch (console)" configuration
5. The debugger will start and stop at your breakpoints

## Project Structure

```
solid-project-1/
├── solid-project-1.csproj    # Project configuration
├── Program.cs                     # Application entry point
└── ...                           # Additional source files
```

## Development Environment Features

### VS Code Configuration
- **IntelliSense:** Full C# code completion and suggestions
- **Debugging:** Integrated debugging with breakpoints and variable inspection
- **Tasks:** Pre-configured build, run, and test tasks
- **Extensions:** Automatically installed C# development extensions

### DevContainer/Codespaces Features
- **High-Performance Environment:** 8 CPU cores, 16GB RAM
- **Pre-installed Extensions:**
  - C# Dev Kit with full language support
  - GitHub Copilot for AI-assisted coding
  - Visual Assist for enhanced productivity
  - Better C# syntax highlighting
  - IL Spy for .NET decompilation
  - NuGet Gallery integration
  - Coverage gutters for test coverage
  - REST Client for API testing

### Continuous Integration
- **Automated Builds:** Every push and pull request triggers automated builds
- **Multi-job Pipeline:** Build, test, code quality, and security scanning
- **Artifact Storage:** Build outputs are stored for 30 days
- **Cross-branch Support:** CI runs on all branches to ensure consistency

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes and commit: `git commit -m "Add your feature"`
4. Push to your fork: `git push origin feature/your-feature-name`
5. Create a Pull Request

## Code Style

This project follows standard C# coding conventions:
- PascalCase for public members and types
- camelCase for private fields and local variables
- Meaningful names for classes, methods, and variables
- XML documentation comments for public APIs

## Troubleshooting

### Common Issues

**Build Errors:**
- Ensure .NET 8.0 SDK is installed
- Run `dotnet restore` to restore NuGet packages
- Check that you're in the correct directory

**VS Code Issues:**
- Install the C# Dev Kit extension
- Reload VS Code window: `Ctrl+Shift+P` → "Developer: Reload Window"
- Check that .NET is properly installed: `dotnet --version`

**Codespaces Issues:**
- Wait for the environment to fully initialize
- If extensions aren't working, try rebuilding the container
- Check the terminal for any error messages during setup

---

Happy coding!
