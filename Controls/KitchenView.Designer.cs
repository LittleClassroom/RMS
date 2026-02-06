namespace RMS.Controls
{
    partial class KitchenView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel ticketsPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenView));
            headerPanel = new Panel();
            lblTitle = new Label();
            ticketsPanel = new FlowLayoutPanel();
            headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(255, 87, 34);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(16, 12, 16, 12);
            headerPanel.Size = new Size(800, 60);
            headerPanel.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(232, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🍳 Kitchen Display";
            // 
            // ticketsPanel
            // 
            ticketsPanel.AutoScroll = true;
            ticketsPanel.BackColor = Color.FromArgb(64, 64, 64);
            ticketsPanel.BackgroundImage = (Image)resources.GetObject("ticketsPanel.BackgroundImage");
            ticketsPanel.Dock = DockStyle.Fill;
            ticketsPanel.Location = new Point(0, 60);
            ticketsPanel.Name = "ticketsPanel";
            ticketsPanel.Padding = new Padding(16);
            ticketsPanel.Size = new Size(800, 440);
            ticketsPanel.TabIndex = 0;
            ticketsPanel.Paint += ticketsPanel_Paint;
            // 
            // KitchenView
            // 
            Controls.Add(ticketsPanel);
            Controls.Add(headerPanel);
            Name = "KitchenView";
            Size = new Size(800, 500);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void AddSampleTickets()
        {
            // Sample tickets - in real app, from database
            var tickets = new[]
            {
                (1001, "T3", "12:34", new[] { "2x Burger", "1x Fries", "1x Caesar Salad" }, 0),  // Pending
                (1002, "T6", "12:38", new[] { "1x Steak Medium", "2x Soup of Day" }, 1),         // Cooking
                (1003, "T2", "12:42", new[] { "3x Fish & Chips", "1x Kids Meal" }, 0),           // Pending
                (1004, "T5", "12:45", new[] { "1x Pasta Carbonara" }, 2),                         // Ready
            };

            foreach (var (orderId, table, time, items, status) in tickets)
            {
                var ticket = CreateTicket(orderId, table, time, items, status);
                this.ticketsPanel.Controls.Add(ticket);
            }
        }

        private System.Windows.Forms.Panel CreateTicket(int orderId, string table, string time, string[] items, int status)
        {
            var statusColors = new System.Drawing.Color[]
            {
                System.Drawing.Color.FromArgb(255, 193, 7),  // Pending - Yellow
                System.Drawing.Color.FromArgb(33, 150, 243), // Cooking - Blue
                System.Drawing.Color.FromArgb(76, 175, 80),  // Ready - Green
            };

            var statusNames = new[] { "⏳ PENDING", "🔥 COOKING", "✅ READY" };

            var ticket = new System.Windows.Forms.Panel
            {
                Width = 220,
                Height = 200,
                Margin = new System.Windows.Forms.Padding(8),
                BackColor = System.Drawing.Color.White,
                Padding = new System.Windows.Forms.Padding(0)
            };

            var headerBar = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 40,
                BackColor = statusColors[status]
            };

            var lblOrderInfo = new System.Windows.Forms.Label
            {
                Text = $"#{orderId}  •  {table}",
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(8, 10),
                AutoSize = true
            };

            var lblTime = new System.Windows.Forms.Label
            {
                Text = time,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(170, 12),
                AutoSize = true
            };

            headerBar.Controls.Add(lblOrderInfo);
            headerBar.Controls.Add(lblTime);

            var itemsPanel = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(0, 40),
                Width = 220,
                Height = 120,
                Padding = new System.Windows.Forms.Padding(8)
            };

            int y = 8;
            foreach (var item in items)
            {
                var lblItem = new System.Windows.Forms.Label
                {
                    Text = $"• {item}",
                    Font = new System.Drawing.Font("Segoe UI", 10F),
                    Location = new System.Drawing.Point(8, y),
                    AutoSize = true
                };
                itemsPanel.Controls.Add(lblItem);
                y += 24;
            }

            var footerBar = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 36,
                BackColor = System.Drawing.Color.FromArgb(245, 245, 245)
            };

            var lblStatus = new System.Windows.Forms.Label
            {
                Text = statusNames[status],
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = statusColors[status],
                Location = new System.Drawing.Point(8, 10),
                AutoSize = true
            };
            footerBar.Controls.Add(lblStatus);

            ticket.Controls.Add(headerBar);
            ticket.Controls.Add(itemsPanel);
            ticket.Controls.Add(footerBar);

            return ticket;
        }
    }
}
