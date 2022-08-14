using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Talamus_ContentManager
{
    public static class FlowDocumentExtensions
    {
        public static IEnumerable<Paragraph> Paragraphs(this FlowDocument doc)
        {
            return doc.Descendants().OfType<Paragraph>();
        }
    }

    public static class DependencyObjectExtensions
    {
        public static IEnumerable<DependencyObject> Descendants(this DependencyObject root)
        {
            if (root == null)
                yield break;
            yield return root;
            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
                foreach (var descendent in child.Descendants())
                    yield return descendent;
        }
    }
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory + "preview.html";
        public string Content { get; set; }
        public EditWindow(string? content = null)
        {
            InitializeComponent();

            if (content == null)
            {
                File.WriteAllText("preview.html", MakeHtmlPage("<--Type somthing"), Encoding.Unicode);

            }
            else
            {
                File.WriteAllText("preview.html", MakeHtmlPage(content), Encoding.Unicode);
                rtb.AppendText(content);
            }
            wbPreview.Navigate(path);
        }

        string MakeHtmlPage(string content)
        {
            string head = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">" +
                "<link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin><link href=\"https://fonts.googleapis.com/css2?family=Roboto:wght@300;" +
                "400;700&display=swap\" rel=\"stylesheet\">";
            string body = "<body style=\"position: relative;background-color: #5e0dac;font-family: 'Roboto' , sans-serif;z-index: -1;\"><div style=\"position: relative;background-color: #3b0470;border-color: #a069d6;border-width: 0px 3px;border-style: solid;padding: 0;z-index: -1;\">";
            string textDiv = "<div style=\"color:white;padding: 20px 35px;font-size: medium;font-weight: 200;line-height: 3.5ex;\">";

            string closeTags = "</div></div></body></html>";

            return head + body + textDiv + content + closeTags;
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string content = "";
            Content = "";
            RichTextBox rtb = (RichTextBox)sender;

            foreach (var paragraph in rtb.Document.Paragraphs())
            {
                TextRange tr = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
                content += "<p>" + tr.Text + "</p>";
                Content += tr.Text + "\n";
            }

            File.WriteAllText("preview.html", MakeHtmlPage(content), Encoding.Unicode);
            wbPreview.Navigate(path);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextRange tr = new TextRange(rtb.Selection.Start, rtb.Selection.End);
            tr.Text = "<span style=\"font-weight: 900;\">" + tr.Text + "</style>";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
