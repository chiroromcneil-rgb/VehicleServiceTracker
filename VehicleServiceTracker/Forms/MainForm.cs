using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleServiceTracker.Models;
using VehicleServiceTracker.Services;

namespace VehicleServiceTracker.Forms
{
    public class MainForm : Form
    {
        private readonly ServiceManager manager;
        private DataGridView vehicleGrid;
        private DataGridView bookingGrid;
        private TextBox searchBox;
        private Label reminderLabel;

        public MainForm()
        {
            manager = new ServiceManager();
            BuildUi();
            RefreshAll();
        }

        private void BuildUi()
        {
            Text = "Vehicle Service Booking and Maintenance Tracker";
            Width = 1100;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Panel header = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(35, 65, 105) };
            Label title = new Label
            {
                Text = "Vehicle Service Booking and Maintenance Tracker",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 18)
            };
            header.Controls.Add(title);
            Controls.Add(header);

            Panel controls = new Panel { Dock = DockStyle.Top, Height = 90 };
            searchBox = new TextBox { Left = 20, Top = 18, Width = 290 };
            Button searchButton = MakeButton("Search", 320, 15, SearchVehicles);
            Button addButton = MakeButton("Add Vehicle", 420, 15, AddVehicle);
            Button editButton = MakeButton("Edit Vehicle", 540, 15, EditVehicle);
            Button deleteButton = MakeButton("Delete Vehicle", 670, 15, DeleteVehicle);
            Button bookingButton = MakeButton("New Booking", 810, 15, AddBooking);
            Button recordButton = MakeButton("Add Service Record", 20, 50, AddMaintenanceRecord);
            Button reportButton = MakeButton("Reports", 180, 50, ShowReports);
            reminderLabel = new Label { Left = 330, Top = 55, Width = 700, ForeColor = Color.DarkGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            controls.Controls.AddRange(new Control[] { searchBox, searchButton, addButton, editButton, deleteButton, bookingButton, recordButton, reportButton, reminderLabel });
            Controls.Add(controls);

            SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 330 };
            vehicleGrid = MakeGrid();
            bookingGrid = MakeGrid();
            split.Panel1.Controls.Add(WrapWithLabel("Vehicles", vehicleGrid));
            split.Panel2.Controls.Add(WrapWithLabel("Service Bookings", bookingGrid));
            Controls.Add(split);
        }

        private Button MakeButton(string text, int left, int top, EventHandler click)
        {
            Button button = new Button { Text = text, Left = left, Top = top, Width = 140, Height = 30 };
            button.Click += click;
            return button;
        }

        private DataGridView MakeGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
        }

        private Control WrapWithLabel(string text, Control grid)
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };
            Label label = new Label { Text = text, Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            panel.Controls.Add(grid);
            panel.Controls.Add(label);
            return panel;
        }

        private void RefreshAll()
        {
            vehicleGrid.DataSource = manager.SearchVehicles(searchBox == null ? "" : searchBox.Text)
                .Select(v => new
                {
                    v.VehicleId,
                    Type = v.VehicleType,
                    v.RegistrationNumber,
                    v.Make,
                    v.Model,
                    v.Year,
                    Owner = v.Owner.Name,
                    Contact = v.Owner.ContactNumber,
                    TotalCost = manager.CalculateTotalCost(v.VehicleId).ToString("C")
                }).ToList();

            bookingGrid.DataSource = manager.Bookings.Select(b => new
            {
                b.BookingId,
                Vehicle = manager.GetVehicleById(b.VehicleId) == null ? "Unknown" : manager.GetVehicleById(b.VehicleId).RegistrationNumber,
                Date = b.BookingDate.ToShortDateString(),
                b.ServiceType,
                b.WorkshopName,
                b.Status
            }).OrderBy(b => b.Date).ToList();

            int upcoming = manager.GetUpcomingBookings().Count;
            reminderLabel.Text = upcoming == 0 ? "No upcoming service reminders." : upcoming + " upcoming service reminder(s).";
        }

        private int SelectedVehicleId()
        {
            if (vehicleGrid.CurrentRow == null) return 0;
            return Convert.ToInt32(vehicleGrid.CurrentRow.Cells["VehicleId"].Value);
        }

        private int SelectedBookingId()
        {
            if (bookingGrid.CurrentRow == null) return 0;
            return Convert.ToInt32(bookingGrid.CurrentRow.Cells["BookingId"].Value);
        }

        private void SearchVehicles(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void AddVehicle(object sender, EventArgs e)
        {
            using (VehicleForm form = new VehicleForm(manager))
            {
                if (form.ShowDialog() == DialogResult.OK) RefreshAll();
            }
        }

        private void EditVehicle(object sender, EventArgs e)
        {
            int id = SelectedVehicleId();
            if (id == 0) { MessageBox.Show("Please select a vehicle first."); return; }
            using (VehicleForm form = new VehicleForm(manager, id))
            {
                if (form.ShowDialog() == DialogResult.OK) RefreshAll();
            }
        }

        private void DeleteVehicle(object sender, EventArgs e)
        {
            try
            {
                int id = SelectedVehicleId();
                if (id == 0) { MessageBox.Show("Please select a vehicle first."); return; }
                if (MessageBox.Show("Delete this vehicle and its bookings/history?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    manager.DeleteVehicle(id);
                    RefreshAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Error");
            }
        }

        private void AddBooking(object sender, EventArgs e)
        {
            using (BookingForm form = new BookingForm(manager, SelectedVehicleId()))
            {
                if (form.ShowDialog() == DialogResult.OK) RefreshAll();
            }
        }

        private void AddMaintenanceRecord(object sender, EventArgs e)
        {
            using (MaintenanceForm form = new MaintenanceForm(manager, SelectedVehicleId()))
            {
                if (form.ShowDialog() == DialogResult.OK) RefreshAll();
            }
        }

        private void ShowReports(object sender, EventArgs e)
        {
            int id = SelectedVehicleId();
            if (id == 0) { MessageBox.Show("Please select a vehicle first."); return; }
            using (ReportForm form = new ReportForm(manager, id))
            {
                form.ShowDialog();
            }
        }
    }
}
