"use strict";
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below

import * as vscode from "vscode";
import * as languageClient from "vscode-languageclient";
import * as path from "path";
import * as fs from "fs";

// Defines the search path of your language server DLL. (.NET Core)
const languageServerPaths = [
	"server/STServer.dll",
	"../../STServer/bin/Debug/netcoreapp2.1/STServer.dll",
]

function activateLanguageServer(context: vscode.ExtensionContext) {
	// The server is implemented in an executable application.
	let serverModule: string = null;
	for (let p of languageServerPaths) {
		p = context.asAbsolutePath(p);
		// console.log(p);
		if (fs.existsSync(p)) {
			serverModule = p;
			break;
		}
	}
	if (!serverModule) throw new URIError("Cannot find the language server module.");
	let workPath = path.dirname(serverModule);
	console.log(`Use ${serverModule} as server module.`);
	console.log(`Work path: ${workPath}.`);


	// If the extension is launched in debug mode then the debug server options are used
	// Otherwise the run options are used
	let serverOptions: languageClient.ServerOptions = {
		run: { command: "dotnet", args: [serverModule], options: { cwd: workPath } },
		debug: { command: "dotnet", args: [serverModule, "--debug"], options: { cwd: workPath } }
	}
	// Options to control the language client
	let clientOptions: languageClient.LanguageClientOptions = {
		// Register the server for plain text documents
		documentSelector: ["st"],
		synchronize: {
			// Synchronize the setting section 'STServerSetting' to the server
			configurationSection: "STServerSetting",
			// Notify the server about file changes to '.clientrc files contain in the workspace
			fileEvents: [
				vscode.workspace.createFileSystemWatcher("**/.clientrc"),
				vscode.workspace.createFileSystemWatcher("**/.st"),
			]
		},
	}

	// Create the language client and start the client.
	let client = new languageClient.LanguageClient("ST Client", "ST Client", serverOptions, clientOptions);
	let disposable = client.start();

	// Push the disposable to the context's subscriptions so that the 
	// client can be deactivated on extension deactivation
	context.subscriptions.push(disposable);
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

	console.log("activate server");

	activateLanguageServer(context);

	console.log("st extension is now activated.");
}

// this method is called when your extension is deactivated
export function deactivate() {
}