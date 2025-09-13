# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the solid-project-1 .NET solution.

## Workflows Overview

### 1. `ci.yml` - Main CI/CD Pipeline
**Triggers:** Push to main/master/develop branches, Pull requests to main/master, Manual dispatch

**Features:**
- ✅ Builds the .NET 8.0 solution
- ✅ Restores NuGet packages with caching
- ✅ Runs unit tests (if any exist)
- ✅ Uploads test results and build artifacts
- ✅ Performs code quality analysis
- ✅ Provides comprehensive build status feedback

**Jobs:**
- `build-and-test`: Main build and test execution
- `code-quality`: Static code analysis
- `build-status`: Summary of all results

### 2. `build-matrix.yml` - Multi-Platform Build
**Triggers:** Push to main/master, Pull requests, Weekly schedule, Manual dispatch

**Features:**
- ✅ Tests builds across Ubuntu, Windows, and macOS
- ✅ Creates platform-specific artifacts
- ✅ Publishes ready-to-run applications
- ✅ Provides cross-platform compatibility validation

### 3. `pr-check.yml` - Pull Request Validation
**Triggers:** Pull request events (opened, synchronized, reopened)

**Features:**
- ✅ Fast validation for pull requests
- ✅ Detects what types of changes were made
- ✅ Provides quick feedback to contributors
- ✅ Generates PR summary with validation results

## Configuration

### Environment Variables
All workflows use these common environment variables:
```yaml
env:
  DOTNET_VERSION: '8.0.x'          # .NET version to use
  SOLUTION_PATH: 'solid-project-1/solid-project-1.sln'  # Path to solution file
  PROJECT_PATH: 'solid-project-1/solid-project-1.csproj'  # Path to main project
```

### Customization
To customize the workflows for your needs:

1. **Change .NET Version**: Update `DOTNET_VERSION` in the env section
2. **Add Test Projects**: The workflows automatically detect and run test projects
3. **Modify Triggers**: Adjust the `on:` section to change when workflows run
4. **Add Code Analysis**: Extend the `code-quality` job with additional tools

## Adding Unit Tests

Currently, no test projects were detected in the solution. To add tests:

```bash
# Navigate to your solution directory
cd solid-project-1

# Create a new test project
dotnet new xunit -n solid-project-1.Tests

# Add the test project to the solution
dotnet sln add solid-project-1.Tests/solid-project-1.Tests.csproj

# Add reference to the main project
cd solid-project-1.Tests
dotnet add reference ../solid-project-1.csproj
```

Once you add test projects, the workflows will automatically:
- Detect and run your tests
- Generate test reports
- Upload test results as artifacts
- Include test coverage information

## Artifacts

The workflows generate several types of artifacts:

### Build Artifacts
- **Name**: `build-artifacts`
- **Contents**: Compiled binaries and build outputs
- **Retention**: 7 days

### Test Results
- **Name**: `test-results-{run-number}`
- **Contents**: Test result files (.trx) and coverage reports
- **Retention**: 30 days

### Platform Builds
- **Names**: `linux-build-{run-number}`, `windows-build-{run-number}`, `macos-build-{run-number}`
- **Contents**: Published applications for each platform
- **Retention**: 7 days

## Status Badges

Add these badges to your README.md to show build status:

```markdown
[![CI/CD Pipeline](https://github.com/oop-jccc/solid-project-1/actions/workflows/ci.yml/badge.svg)](https://github.com/oop-jccc/solid-project-1/actions/workflows/ci.yml)
[![Multi-Platform Build](https://github.com/oop-jccc/solid-project-1/actions/workflows/build-matrix.yml/badge.svg)](https://github.com/oop-jccc/solid-project-1/actions/workflows/build-matrix.yml)
[![PR Check](https://github.com/oop-jccc/solid-project-1/actions/workflows/pr-check.yml/badge.svg)](https://github.com/oop-jccc/solid-project-1/actions/workflows/pr-check.yml)
```

## Troubleshooting

### Common Issues

1. **Build Failures**: Check that all NuGet packages are properly restored
2. **Test Failures**: Ensure test projects have proper references to the main project
3. **Permission Issues**: Verify that GitHub Actions has the necessary permissions

### Debugging

- Check the Actions tab in your GitHub repository
- Review the detailed logs for each workflow step
- Use the workflow dispatch feature to manually trigger builds for testing

## Best Practices

1. **Keep workflows fast**: The PR check workflow is optimized for speed
2. **Use caching**: NuGet package caching reduces build times
3. **Fail fast**: The build matrix uses `fail-fast: false` to see all platform results
4. **Clear feedback**: All workflows provide detailed status summaries
5. **Artifact management**: Artifacts have appropriate retention periods to manage storage
