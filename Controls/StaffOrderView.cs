using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.Controls
{
    public class StaffOrderView : UserControl
    {
        private IContainer components;
        private SplitContainer splitMain;
        private Panel placeholderPanel;
        private Label placeholderLabel;
        private TableMapView tableMapView;
        private OrderPanel? currentOrderPanel;

        public StaffOrderView()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            tableMapView = new TableMapView { Dock = DockStyle.Fill };
            tableMapView.TableSelected += TableMapView_TableSelected;
            splitMain.Panel1.Controls.Add(tableMapView);
            ShowPlaceholder("Select a table to start an order");
        }

        private void InitializeComponent()
        {
            components = new Container();
            splitMain = new SplitContainer();
            placeholderPanel = new Panel();
            placeholderLabel = new Label();
            ((ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            placeholderPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            splitMain.Panel1MinSize = 300;
            splitMain.Panel2.Controls.Add(placeholderPanel);
            splitMain.Panel2MinSize = 400;
            splitMain.Size = new Size(1159, 883);
            splitMain.SplitterDistance = 755;
            splitMain.TabIndex = 0;
            // 
            // placeholderPanel
            // 
            placeholderPanel.BackColor = Color.White;
            placeholderPanel.Controls.Add(placeholderLabel);
            placeholderPanel.Dock = DockStyle.Fill;
            placeholderPanel.Location = new Point(0, 0);
            placeholderPanel.Name = "placeholderPanel";
            placeholderPanel.Size = new Size(400, 883);
            placeholderPanel.TabIndex = 0;
            // 
            // placeholderLabel
            // 
            placeholderLabel.Dock = DockStyle.Fill;
            placeholderLabel.Font = new Font("Segoe UI", 14F);
            placeholderLabel.ForeColor = Color.Gray;
            placeholderLabel.TextAlign = ContentAlignment.MiddleCenter;
            placeholderLabel.Name = "placeholderLabel";
            // 
            // StaffOrderView
            // 
            Controls.Add(splitMain);
            Name = "StaffOrderView";
            Size = new Size(1159, 883);
            splitMain.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            placeholderPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ShowPlaceholder(string message)
        {
            placeholderLabel.Text = message;
            splitMain.Panel2.Controls.Clear();
            splitMain.Panel2.Controls.Add(placeholderPanel);
        }

        private void ShowLoading()
        {
            var loadingLabel = new Label
            {
                Text = "Loading order...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.Gray
            };
            splitMain.Panel2.Controls.Clear();
            splitMain.Panel2.Controls.Add(loadingLabel);
        }

        private async void TableMapView_TableSelected(object? sender, TableSelectedEventArgs e)
        {
            ShowLoading();

            if (currentOrderPanel != null)
            {
                currentOrderPanel.OrderCompleted -= OrderPanel_OrderCompleted;
            }

            currentOrderPanel = new OrderPanel { Dock = DockStyle.Fill };
            currentOrderPanel.OrderCompleted += OrderPanel_OrderCompleted;
            splitMain.Panel2.Controls.Clear();
            splitMain.Panel2.Controls.Add(currentOrderPanel);

            try
            {
                await currentOrderPanel.LoadTableOrder(e.Table);
            }
            catch (Exception ex)
            {
                ShowPlaceholder($"Failed to load order: {ex.Message}");
            }
        }

        private void OrderPanel_OrderCompleted(object? sender, OrderCompletedEventArgs e)
        {
            if (currentOrderPanel != null)
            {
                currentOrderPanel.OrderCompleted -= OrderPanel_OrderCompleted;
                currentOrderPanel = null;
            }

            string orderLabel = e.OrderId.HasValue ? $"Order #{e.OrderId}" : "Order";
            string message = e.CompletionType switch
            {
                OrderCompletionType.Checkout => $"💰 {orderLabel} checked out.\n\nSelect another table to continue.",
                OrderCompletionType.Submitted => $"✅ {orderLabel} sent to kitchen!\n\nSelect another table to continue.",
                _ => "Order closed. Select another table to continue."
            };
            ShowPlaceholder(message);

            tableMapView.RefreshTables();
        }
    }
}