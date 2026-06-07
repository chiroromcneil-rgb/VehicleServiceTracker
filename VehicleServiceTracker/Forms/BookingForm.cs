using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleServiceTracker.Models;
using VehicleServiceTracker.Services;

namespace VehicleServiceTracker.Forms
{
    public class BookingForm : Form
    {
        private readonly ServiceManager manager;
        private ComboBox vehicleCombo, statusCombo;
        private DateTimePicker datePicker;
        private TextBox serviceTypeBox, workshopBox;

        public BookingForm(ServiceManager manager, int selectedVehicleId = 0)
        {
            this.manager = manager;
            BuildUi(selectedVehicleId);
        }

        private void BuildUi(int selectedVehicleId)
        {
            Text = "Create Service Booking";
            Width = 500;
            Height = 330;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);

            AddLabel("Vehicle", 25, 25);
            vehicleCombo = new ComboBox { Left = 180, Top = 22, Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            vehicleCombo.DataSource = manager.Vehicles.Select(v => new { v.VehicleId, Display = v.GetDisplayName() }).ToList();
            vehicleCombo.DisplayMember = "Display";
            vehicleCombo.ValueMember = "VehicleId";
            if (selectedVehicleId > 0) vehicleCombo.SelectedValue = selectedVehicleId;
            Controls.Add(vehicleCombo);

            AddLabel("Booking Date", 25, 75);
            datePicker = new DateTimePicker { Left = 180, Top = 72, Width = 270, Format = DateTimePickerFormat.Short };
            Controls.Add(datePicker);

            AddLabel("Service Type", 25, 125);
            serviceTypeBox = new TextBox { Left = 180, Top = 122, Width = 270 };
            Controls.Add(serviceTypeBox);

            AddLabel("Workshop Name", 25, 175);
            workshopBox = new TextBox { Left = 180, Top = 172, Width = 270 };
            Controls.Add(workshopBox);

            AddLabel("Status", 25, 225);
            statusCombo = new ComboBox { Left = 180, Top = 222, Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            statusCombo.Items.AddRange(new object[] { "Booked", "Completed", "Cancelled" });
            statusCombo.SelectedIndex = 0;
            Controls.Add(statusCombo);

            Button save = new Button { Text = "Save", Left = 280, Top = 260, Width = 80 };
            Button cancel = new Button { Text = "Cancel", Left = 370, Top = 260, Width = 80 };
            save.Click += SaveBooking;
            cancel.Click += (s, e) => Close();
            Controls.Add(save);
            Controls.Add(cancel);
        }

        private void AddLabel(string text, int left, int top)
        {
            Controls.Add(new Label { Text = text, Left = left, Top = top, Width = 140 });
        }

        private void SaveBooking(object sender, EventArgs e)
        {
            try
            {
                if (vehicleCombo.SelectedValue == null) throw new InvalidOperationException("Please add/select a vehicle first.");
                ServiceBooking booking = new ServiceBooking
                {
                    VehicleId = Convert.ToInt32(vehicleCombo.SelectedValue),
                    BookingDate = datePicker.Value.Date,
                    ServiceType = serviceTypeBox.Text,
                    WorkshopName = workshopBox.Text,
                    Status = statusCombo.Text
                };
                manager.AddBooking(booking);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Booking Error");
            }
        }
    }
}
