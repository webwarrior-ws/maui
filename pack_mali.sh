#!/bin/env bash

# Rename packages from Microsoft.Maui.* to Mali.* and run dotnet pack on them.
# Use PROJECTS_TO_PACK env. var for newline-separated list of projects.

set -euxo pipefail

# when we change PackageId we also have to change file names in buildTransitive dir
find src/Core/src/nuget/buildTransitive -type f -name 'Microsoft.Maui.*' -print | while read -r file; do
    newname=${file/Microsoft.Maui./Mali.}
    mv "$file" "$newname"
done

cd src/BlazorWebView/src/Maui/build
mv Microsoft.AspNetCore.Components.WebView.Maui.props Mali.AspNetCore.Components.WebView.props
mv Microsoft.AspNetCore.Components.WebView.Maui.targets Mali.AspNetCore.Components.WebView.targets
cd ../../../../..
    
echo "$PROJECTS_TO_PACK" | while read -r project; do
    [ -z "$project" ] && continue  # Skip if empty
    # change PackagIds to Mali.*
    if [ "$project" = "src/BlazorWebView/src/Maui/Microsoft.AspNetCore.Components.WebView.Maui.csproj" ]; then
        sed -i 's/<PropertyGroup>/<PropertyGroup>\n<PackageId>Mali.AspNetCore.Components.WebView<\/PackageId>/g' "$project"
    elif grep -q "<PackageId>Microsoft.Maui" "$project"; then
        sed -i 's/<PackageId>Microsoft.Maui/<PackageId>Mali/g' "$project"
    else
        sed -i -r 's/<AssemblyName>Microsoft.Maui.([^<]+)<\/AssemblyName>/<PackageId>Mali.\1<\/PackageId>\n&/g' "$project"
    fi
    dotnet pack "$project" --no-build --no-restore
done

