namespace odbc_config
{
    partial class SearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchResults = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // SearchText
            // 
            this.SearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchText.Location = new System.Drawing.Point(12, 12);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(784, 20);
            this.SearchText.TabIndex = 0;
            this.SearchText.Text = "Search...";
            this.SearchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            this.SearchText.Enter += new System.EventHandler(this.searchText_Enter);
            this.SearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyUp);
            this.SearchText.Leave += new System.EventHandler(this.searchText_Leave);
            // 
            // SearchResults
            // 
            this.SearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchResults.FormattingEnabled = true;
            this.SearchResults.Location = new System.Drawing.Point(13, 39);
            this.SearchResults.Name = "SearchResults";
            this.SearchResults.Size = new System.Drawing.Size(783, 407);
            this.SearchResults.TabIndex = 1;
            this.SearchResults.DoubleClick += new System.EventHandler(this.searchResults_DoubleClick);
            this.SearchResults.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchResults_KeyUp);
            // 
            // SearchForm
            // 
            this.ClientSize = new System.Drawing.Size(808, 458);
            this.Controls.Add(this.SearchResults);
            this.Controls.Add(this.SearchText);
            this.Name = "SearchForm";
            this.Text = "ODBC Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.ListBox SearchResults;
    }
}

