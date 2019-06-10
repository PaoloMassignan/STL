using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;
using Newtonsoft.Json.Linq;

namespace STL.Server.Services
{
    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public class TextDocumentService : DemoLanguageServiceBase
    {
        [JsonRpcMethod]
        public async Task<Hover> Hover(TextDocumentIdentifier textDocument, Position position, CancellationToken ct)
        {
            // Note that Hover is cancellable.
            await Task.Delay(1000, ct);
            return new Hover { Contents = "Test _hover_ @" + position + "\n\n" + textDocument };
        }

        [JsonRpcMethod]
        public SignatureHelp SignatureHelp(TextDocumentIdentifier textDocument, Position position)
        {
            return new SignatureHelp(new List<SignatureInformation>
            {
                new SignatureInformation("**Function1**", "Documentation1"),
                new SignatureInformation("**Function2** <strong>test</strong>", "Documentation2"),
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidOpen(TextDocumentItem textDocument)
        {
            try
            {

                var doc = new SessionDocument(textDocument);
                var session = Session;
                doc.DocumentChanged += async (sender, args) =>
                {
                    Program.logWriter.WriteLine("Document changed");
                    // Lint the document when it's changed.
                    var doc1 = ((SessionDocument)sender).Document;
                    var diag1 = session.DiagnosticProvider.LintDocument(doc1, session.Settings.MaxNumberOfProblems);
                    var colors = session.ColorProvider.ColorDocument(doc1);
                    if (session.Documents.ContainsKey(doc1.Uri))
                    {
                        // In case the document has been closed when we were linting…
                        await session.Client.Document.PublishDiagnostics(doc1.Uri, diag1);
                        await session.Client.Document.ColorProvider(doc1.Uri, colors);
                    }
                };
                if (Session == null)
                    Program.logWriter.WriteLine("Session is null");
                if (Session.Documents == null)
                    Program.logWriter.WriteLine("Session.Documents is null");

                Session.Documents.TryAdd(textDocument.Uri, doc);

                if (Session.DiagnosticProvider == null)
                    Program.logWriter.WriteLine("Session.DiagnosticProvider is null");

                if (Session.Settings == null)
                    Program.logWriter.WriteLine("Session.Settings is null");

                if (doc.Document == null)
                    Program.logWriter.WriteLine("doc.Document is null");


                var diag = Session.DiagnosticProvider.LintDocument(doc.Document, Session.Settings.MaxNumberOfProblems);
                await Client.Document.PublishDiagnostics(textDocument.Uri, diag);

                var color = Session.ColorProvider.ColorDocument(doc.Document);
                await Client.Document.ColorProvider(textDocument.Uri, color);

            }
            catch (Exception exc)
            {
                Program.logWriter.WriteLine(exc.Message + "\r\n" + exc.StackTrace);

            }
        }

        [JsonRpcMethod(IsNotification = true)]
        public void DidChange(TextDocumentIdentifier textDocument,
            ICollection<TextDocumentContentChangeEvent> contentChanges)
        {
            Program.logWriter.WriteLine("Documento changed");
            if (Session.Documents[textDocument.Uri] == null)
            {
                Program.logWriter.WriteLine("Unknown doc");
                var item = new TextDocumentItem();
                item.Uri = textDocument.Uri;
                var doc = new SessionDocument(item);
                var session = Session;
                doc.DocumentChanged += async (sender, args) =>
                {
                    Program.logWriter.WriteLine("Document changed");
                    // Lint the document when it's changed.
                    var doc1 = ((SessionDocument)sender).Document;
                    var diag1 = session.DiagnosticProvider.LintDocument(doc1, session.Settings.MaxNumberOfProblems);
                    if (session.Documents.ContainsKey(doc1.Uri))
                    {
                        // In case the document has been closed when we were linting…
                        await session.Client.Document.PublishDiagnostics(doc1.Uri, diag1);
                    }
                };
                Session.Documents.TryAdd(textDocument.Uri, doc);
            }
            Program.logWriter.WriteLine("A");

            Session.Documents[textDocument.Uri].NotifyChanges(contentChanges);
            Program.logWriter.WriteLine("B");

        }

        [JsonRpcMethod(IsNotification = true)]
        public void WillSave(TextDocumentIdentifier textDocument, TextDocumentSaveReason reason)
        {
            //Client.Window.LogMessage(MessageType.Log, "-----------");
            //Client.Window.LogMessage(MessageType.Log, Documents[textDocument].Content);
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidClose(TextDocumentIdentifier textDocument)
        {
            if (textDocument.Uri.IsUntitled())
            {
                await Client.Document.PublishDiagnostics(textDocument.Uri, new Diagnostic[0]);
            }
            Session.Documents.TryRemove(textDocument.Uri, out _);
        }

        [JsonRpcMethod]
        public CompletionList Completion(TextDocumentIdentifier textDocument, Position position, CompletionContext context)
        {
            Program.logWriter.WriteLine("Completion requested");
            CompletionList list = new CompletionList();
            try
            {
                if (DiagnosticProvider.Compiler == null)
                    return list;

                foreach (var token in DiagnosticProvider.Compiler.GetExpectedTokens())
                {
                    Program.logWriter.WriteLine(token);
                    list.Items.Add(new CompletionItem(token, CompletionItemKind.Keyword, token));
                }
            }
            catch (Exception exc)
            {
                Program.logWriter.WriteLine(exc.Message+"\r\n"+exc.StackTrace);
            }
            return list;
        }


        //[JsonRpcMethod]
        //public ColorInformation[] DocumentColor(TextDocumentIdentifier textDocument)
        //{

        //    Program.logWriter.WriteLine("color requested");

        //    return new ColorInformation[] { new ColorInformation() { color = new Color(255, 0, 0, 255), range = new Range(1, 1, 1, 2) } };
        //}
    }
}
