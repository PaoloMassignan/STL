﻿using System;
using System.Threading.Tasks;
using JsonRpc.Standard;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Contracts.Client;
using Newtonsoft.Json.Linq;

namespace STL.Server.Services
{
    public class InitializaionService : DemoLanguageServiceBase
    {

        [JsonRpcMethod(AllowExtensionData = true)]
        public InitializeResult Initialize(int processId, Uri rootUri, ClientCapabilities capabilities,
            JToken initializationOptions = null, string trace = null)
        {
            return new InitializeResult(new ServerCapabilities
            {
                HoverProvider = true,
                SignatureHelpProvider = new SignatureHelpOptions("()"),
                CompletionProvider = new CompletionOptions(true, "."),
               // ColorProvider = true,
                TextDocumentSync = new TextDocumentSyncOptions
                {
                    OpenClose = true,
                    WillSave = true,
                    Change = TextDocumentSyncKind.Incremental
                },
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task Initialized()
        {
        }

        [JsonRpcMethod]
        public void Shutdown()
        {

        }

        [JsonRpcMethod(IsNotification = true)]
        public void Exit()
        {
            Session.StopServer();
        }

        [JsonRpcMethod("$/cancelRequest", IsNotification = true)]
        public void CancelRequest(MessageId id)
        {
            RequestContext.Features.Get<IRequestCancellationFeature>().TryCancel(id);
        }
    }
}
