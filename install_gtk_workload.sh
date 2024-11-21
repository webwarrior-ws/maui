#!/usr/bin/env bash

set -euxo pipefail

# For some reason automatic workload manifest detection doesn't work (see https://github.com/GtkSharp/GtkSharp/issues/355#issuecomment-1446262239), so download and uzip mainfest file manually
dotnet nuget add source --name nuget.org "https://api.nuget.org/v3/index.json"
wget https://www.nuget.org/api/v2/package/gtksharp.net.sdk.gtk.manifest-$GtkSharpManifestVersion/$GtkSharpVersion -O gtksharp.net.sdk.gtk.manifest-$GtkSharpManifestVersion.nupkg            
DOTNET_DIR=/home/runner/.dotnet
WORKLOAD_MANIFEST_DIR=$DOTNET_DIR/sdk-manifests/$DotnetVersion/gtksharp.net.sdk.gtk
mkdir -p $WORKLOAD_MANIFEST_DIR
unzip -j gtksharp.net.sdk.gtk.manifest-$GtkSharpManifestVersion.nupkg "data/*" -d $WORKLOAD_MANIFEST_DIR/
rm gtksharp.net.sdk.gtk.manifest-$GtkSharpManifestVersion.nupkg
chmod 764 $WORKLOAD_MANIFEST_DIR/*
dotnet workload search
dotnet workload install gtk --skip-manifest-update
