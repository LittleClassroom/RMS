using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.UI
{
    public sealed class OrderDetailsDialog : Form
    {
        public OrderDetailsDialog(OrderSummary summary, IReadOnlyList<OrderLineDetail> lines)
        {
            Text = $"Order #{summary.OrderId} Details";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Width = 540;
            Height = 540;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(8)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
            Controls.Add(layout);

            var summaryPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 4,
                AutoSize = true
            };
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            layout.Controls.Add(summaryPanel, 0, 0);

            void AddRow(string label, string value, int row)
            {
                var lbl = new Label
                {
                    Text = label,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                };
                var val = new Label
                {
                    Text = value,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                summaryPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
                summaryPanel.Controls.Add(lbl, 0, row);
                summaryPanel.Controls.Add(val, 1, row);
            }

            AddRow("Table", string.IsNullOrWhiteSpace(summary.TableCode) ? "N/A" : summary.TableCode, 0);
            AddRow("Location", string.IsNullOrWhiteSpace(summary.Location) ? "-" : summary.Location!, 1);
            AddRow("Status", GetStatusText(summary.Status), 2);
            AddRow("Created", summary.CreatedAtUtc.ToLocalTime().ToString("g"), 3);

            var totalsPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Bottom,
                Height = 70
            };
            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            totalsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            totalsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            totalsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            summaryPanel.Controls.Add(totalsPanel);
            summaryPanel.SetColumnSpan(totalsPanel, 2);

            void AddTotal(string label, decimal amount, int row)
            {
                var lbl = new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                var val = new Label { Text = amount.ToString("C"), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
                totalsPanel.Controls.Add(lbl, 0, row);
                totalsPanel.Controls.Add(val, 1, row);
            }

            AddTotal("Subtotal", summary.Subtotal, 0);
            AddTotal("Tax", summary.Tax, 1);
            AddTotal("Total", summary.Total, 2);

            var listContainer = new Panel { Dock = DockStyle.Fill };
            layout.Controls.Add(listContainer, 0, 1);

            var lvLines = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                HideSelection = false
            };
            lvLines.Columns.Add("Item", 220, HorizontalAlignment.Left);
            lvLines.Columns.Add("Qty", 60, HorizontalAlignment.Center);
            lvLines.Columns.Add("Unit", 100, HorizontalAlignment.Right);
            lvLines.Columns.Add("Line Total", 110, HorizontalAlignment.Right);
            listContainer.Controls.Add(lvLines);

            foreach (var line in lines)
            {
                var item = new ListViewItem(line.Name);
                item.SubItems.Add(line.Quantity.ToString());
                item.SubItems.Add(line.UnitPrice.ToString("C"));
                item.SubItems.Add((line.UnitPrice * line.Quantity).ToString("C"));
                lvLines.Items.Add(item);
            }

            if (lines.Count == 0)
            {
                var placeholder = new Label
                {
                    Text = "No line items recorded for this order.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray
                };
                listContainer.Controls.Add(placeholder);
                placeholder.BringToFront();
            }

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 6, 0, 0)
            };
            layout.Controls.Add(buttonPanel, 0, 2);
            var btnClose = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.OK,
                AutoSize = true
            };
            btnClose.Click += (_, __) => Close();
            buttonPanel.Controls.Add(btnClose);
            AcceptButton = btnClose;
        }

        private static string GetStatusText(byte status) => status switch
        {
            1 => "In Kitchen",
            2 => "Ready",
            3 => "Closed",
            4 => "Cancelled",
            _ => "Created"
        };
    }
}
