﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Triggleh
{
    public partial class Form1 : Form, IFormGUI
    {
        private delegate void SafeCallDelegate(string text);
        private delegate void SafeCallDelegateBool(bool boolean);
        private FormPresenter presenter;

        public Form1()
        {
            InitializeComponent();
            new Logger();

            RefreshCharAnimStatus();
        }

        public void Register(FormPresenter FP)
        {
            presenter = FP;
        }

        public string TriggerName
        {
            set { txt_TriggerName.Text = value; }
            get { return txt_TriggerName.Text; }
        }

        public bool BitsEnabled
        {
            set { chk_Bits.Checked = value; }
            get { return chk_Bits.Checked; }
        }

        public int BitsCondition
        {
            set { cmb_Bits.SelectedIndex = value; }
            get { return cmb_Bits.SelectedIndex; }
        }

        public bool BitsConditionEnabled
        {
            set { cmb_Bits.Enabled = value; }
            get { return cmb_Bits.Enabled; }
        }

        public int BitsAmount1
        {
            set { nud_Bits1.Value = value; }
            get { return (int) nud_Bits1.Value; }
        }

        public bool BitsAmount1Enabled
        {
            set { nud_Bits1.Enabled = value; }
            get { return nud_Bits1.Enabled; }
        }

        public int BitsAmount2
        {
            set { nud_Bits2.Value = value; }
            get { return (int) nud_Bits2.Value; }
        }

        public bool BitsAmount2Visible
        {
            set { nud_Bits2.Visible = value; }
            get { return nud_Bits2.Visible; }
        }

        public string BitsInfo1
        {
            set { lbl_BitsInfo1.Text = value; }
            get { return lbl_BitsInfo1.Text; }
        }

        public bool BitsInfo2
        {
            set { lbl_BitsInfo2.Visible = value; }
            get { return lbl_BitsInfo2.Visible; }
        }

        public bool UserLevelEveryone
        {
            set { chk_ULEveryone.Checked = value; }
            get { return chk_ULEveryone.Checked; }
        }

        public bool UserLevelSubs
        {
            set { chk_ULSubs.Checked = value; }
            get { return chk_ULSubs.Checked; }
        }
        public bool UserLevelSubsEnabled
        {
            set { chk_ULSubs.Enabled = value; }
            get { return chk_ULSubs.Enabled; }
        }

        public bool UserLevelMods
        {
            set { chk_ULMods.Checked = value; }
            get { return chk_ULMods.Checked; }
        }

        public bool UserLevelModsEnabled
        {
            set { chk_ULMods.Enabled = value; }
            get { return chk_ULMods.Enabled; }
        }

        public string Keyword
        {
            set { txt_Keywords.Text = value; }
            get { return txt_Keywords.Text; }
        }

        public int AddKeyword(string keyword)
        {
            txt_Keywords.Clear();
            return lst_Keywords.Items.Add(keyword);
        }

        public bool HasKeyword(string keyword)
        {
            foreach (string item in lst_Keywords.Items)
            {
                if (item.ToLower() == keyword.ToLower()) return true;
                else continue;
            }

            return false;
        }

        public void RemoveKeyword(int index)
        {
            lst_Keywords.Items.RemoveAt(index);
            lst_Keywords.SelectedIndex = lst_Keywords.Items.Count - 1;
        }

        public string GetKeywords()
        {
            List<string> keywords = lst_Keywords.Items.Cast<string>().ToList();
            return JsonConvert.SerializeObject(keywords);
        }

        public void ClearKeywords()
        {
            txt_Keywords.Clear();
            lst_Keywords.Items.Clear();
        }

        public int KeywordsIndex
        {
            set { lst_Keywords.SelectedIndex = value; }
            get { return lst_Keywords.SelectedIndex; }
        }

        public string CharAnimTriggerKeyChar
        {
            set { lbl_CHTriggerKey.Text = value; }
            get { return lbl_CHTriggerKey.Text; }
        }

        public int CharAnimTriggerKeyValue { get; set; }

        public int Cooldown
        {
            set { nud_Cooldown.Value = value; }
            get { return (int) nud_Cooldown.Value; }
        }

        public int CooldownUnit
        {
            set { cmb_CooldownUnit.SelectedIndex = value; }
            get { return cmb_CooldownUnit.SelectedIndex; }
        }

        public string LastTriggered
        {
            set { lbl_LastTriggered.Text = value; }
            get { return lbl_LastTriggered.Text; }
        }

        public void ResetDetails()
        {
            TriggerName = "";
            BitsEnabled = false;
            EnableBits(false);
            UserLevelEveryone = true;
            AllowSubsMods(false);
            ClearKeywords();
            CharAnimTriggerKeyChar = "None";
            CharAnimTriggerKeyValue = -1;
            Cooldown = 30;
            CooldownUnit = 0;
            LastTriggered = "Never";
            ResetButtonVisible(false);

            ShowChangesMade(false);
        }

        public void EnableBits(bool enabled)
        {
            BitsCondition = (enabled) ? 0 : -1;
            BitsConditionEnabled = enabled;
            BitsAmount1 = 0;
            BitsAmount1Enabled = enabled;
            BitsInfo1 = "bits";
            BitsAmount2 = 0;
            BitsAmount2Visible = false;
            BitsInfo2 = false;
        }

        public void EnableBitsBetween(bool enabled)
        {
            BitsInfo1 = (enabled) ? "and" : "bits";
            BitsAmount2 = 0;
            BitsAmount2Visible = enabled;
            BitsInfo2 = enabled;
        }

        public void AllowSubsMods(bool allowed)
        {
            UserLevelSubs = false;
            UserLevelSubsEnabled = allowed;
            UserLevelMods = false;
            UserLevelModsEnabled = allowed;
        }

        public void SetKeywordIndex(int index)
        {
            lst_Keywords.SelectedIndex = index;
        }

        public void SetAddKeywordAccept()
        {
            ActiveForm.AcceptButton = btn_AddKeyword;
        }

        public bool RecordingTrigger { get; set; }

        public void StartRecordingTrigger()
        {
            if (RecordingTrigger)
            {
                StopRecordingTrigger();
                return;
            };

            RecordingTrigger = true;
            btn_RecordTrigger.Text = "Waiting...";
            btn_RecordTrigger.KeyDown += Btn_RecordTrigger_KeyDown;
        }

        public void StopRecordingTrigger()
        {
            btn_RecordTrigger.KeyDown -= Btn_RecordTrigger_KeyDown;
            btn_RecordTrigger.Text = "Record";
            RecordingTrigger = false;
        }

        public void ClearTriggers()
        {
            dgv_Triggers.Rows.Clear();
        }

        public void PopulateTrigger(Trigger trigger)
        {
            dgv_Triggers.Rows.Add(trigger.Name);
        }

        public void PopulateTriggerDetails(Trigger trigger)
        {
            TriggerName = trigger.Name;
            BitsEnabled = trigger.BitsEnabled;
            EnableBits(trigger.BitsEnabled);
            BitsCondition = trigger.BitsCondition;
            BitsAmount1 = trigger.BitsAmount;
            BitsAmount2 = trigger.BitsAmount2;
            UserLevelEveryone = trigger.UserLevelEveryone;
            AllowSubsMods(!trigger.UserLevelEveryone);
            UserLevelSubs = trigger.UserLevelSubs;
            UserLevelMods = trigger.UserLevelMods;

            ClearKeywords();
            List<string> keywords = JsonConvert.DeserializeObject<List<string>>(trigger.Keywords);
            foreach (string keyword in keywords)
                AddKeyword(keyword);

            CharAnimTriggerKeyChar = trigger.CharAnimTriggerKeyChar;
            CharAnimTriggerKeyValue = trigger.CharAnimTriggerKeyValue;
            Cooldown = trigger.Cooldown;
            CooldownUnit = trigger.CooldownUnit;
            LastTriggered = (trigger.LastTriggered == DateTime.MinValue) ? "Never" : trigger.LastTriggered.ToString();
            ResetButtonVisible(trigger.LastTriggered != DateTime.MinValue);

            ShowChangesMade(false);
        }

        public void SetSelectedTrigger(int index)
        {
            dgv_Triggers.ClearSelection();
            if (index == -1) return;
            dgv_Triggers.Rows[index].Selected = true;
            Dgv_CurrentRow = index;
        }

        public string GetSelectedTrigger()
        {
            if (dgv_Triggers.SelectedRows.Count <= 0) return null;
            return dgv_Triggers.SelectedRows[0].Cells["dgv_Name"].Value.ToString();
        }

        public int GetNumberRows()
        {
            return dgv_Triggers.Rows.Count;
        }

        public void ShowError(string label, bool showing)
        {
            Label lbl = new Label();
            switch (label)
            {
                case "name":
                    lbl = lbl_TriggerName;
                    break;
                case "bits":
                    lbl = lbl_Bits;
                    break;
                case "userlevel":
                    lbl = lbl_UserLevel;
                    break;
                case "keywords":
                    lbl = lbl_Keywords;
                    break;
                case "chtrigger":
                    lbl = lbl_CHTrigger;
                    break;
                case "chat":
                    lbl = lbl_ChatStatus;
                    break;
            }

            if (showing)
            {
                lbl.ForeColor = Color.Red;
            }
            else
            {
                lbl.ForeColor = SystemColors.ControlText;
            }
        }

        public void ShowValidationError(bool showing)
        {
            if (showing)
            {
                lbl_ValidationError.ForeColor = Color.Red;
                lbl_ValidationError.Visible = true;
            }
            else lbl_ValidationError.Visible = false;
        }

        public void RefreshCharAnimStatus()
        {
            if (SendKeystroke.CharAnimRunning)
            {
                lbl_CharAnimStatus.ForeColor = Color.ForestGreen;
                lbl_CharAnimStatus.Text = "Character Animator found!";
            }

            else
            {
                lbl_CharAnimStatus.ForeColor = Color.Red;
                lbl_CharAnimStatus.Text = "Character Animator missing :(";
            }
        }

        public void ShowSettingsForm()
        {
            SettingsForm f1 = new SettingsForm();
            new SettingsPresenter(f1);
            f1.ShowDialog();
            presenter.LoadFromSettings();
        }

        public void SetProfilePicture(string url)
        {
            pic_ProfilePicture.ImageLocation = url;
        }

        public void UpdateChatStatus(int status)
        {
            switch (status)
            {
                case 0:
                    lbl_ChatStatus.ForeColor = Color.Red;
                    WriteChatStatus("Chat disconnected");
                    break;
                case 1:
                    lbl_ChatStatus.ForeColor = Color.ForestGreen;
                    WriteChatStatus("Chat connected!");
                    break;
            }
        }

        private void WriteChatStatus(string status)
        {
            if (lbl_ChatStatus.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteChatStatus);
                Invoke(d, new object[] { status });
            }
            else
            {
                lbl_ChatStatus.Text = status;
            }
        }

        public void SetLastTriggered(string time)
        {
            if (lbl_LastTriggered.InvokeRequired)
            {
                var d = new SafeCallDelegate(SetLastTriggered);
                Invoke(d, new object[] { time });
            }
            else
            {
                lbl_LastTriggered.Text = time;
            }
        }

        public void ResetButtonVisible(bool showing)
        {
            if (btn_ResetLastTriggered.InvokeRequired)
            {
                var d = new SafeCallDelegateBool(ResetButtonVisible);
                Invoke(d, new object[] { showing });
            }
            else
            {
                btn_ResetLastTriggered.Visible = showing;
            }
        }

        public void ShowChangesMade(bool showing)
        {
            if (showing)
            {
                lbl_UnsavedChanges.ForeColor = Color.Red;
                lbl_UnsavedChanges.Visible = true;
            }
            else lbl_UnsavedChanges.Visible = false;
        }

        private void Chk_Bits_CheckedChanged(object sender, EventArgs e)
        {
            presenter.Chk_Bits_CheckedChanged();
        }

        private void Cmb_Bits_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.Cmb_Bits_SelectedIndexChanged();
        }

        private void Chk_ULEveryone_CheckedChanged(object sender, EventArgs e)
        {
            presenter.Chk_ULEveryone_CheckedChanged();
        }

        private void Txt_Keywords_Enter(object sender, EventArgs e)
        {
            presenter.Txt_Keywords_Enter();
        }

        private void Btn_AddKeyword_Click(object sender, EventArgs e)
        {
            presenter.Btn_AddKeyword_Click();
        }

        private void Btn_RemoveKeyword_Click(object sender, EventArgs e)
        {
            presenter.Btn_RemoveKeyword_Click();
        }

        private void Btn_RecordTrigger_Click(object sender, EventArgs e)
        {
            presenter.Btn_RecordTrigger_Click();
        }

        private void Btn_RecordTrigger_KeyDown(object sender, KeyEventArgs e)
        {
            presenter.Btn_RecordTrigger_KeyDown(e.KeyCode.ToString(), e.KeyValue);
        }

        private void Btn_SaveTrigger_Click(object sender, EventArgs e)
        {
            presenter.Btn_SaveTrigger_Click();
        }

        public int Dgv_CurrentRow { get; set; }

        private void Dgv_Triggers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            // Dgv_CurrentRow = e.RowIndex;
            SetSelectedTrigger(e.RowIndex);
            presenter.Dgv_Triggers_CellClick(dgv_Triggers.Rows[e.RowIndex].Cells["dgv_Name"].Value.ToString());
        }

        private void Btn_AddTrigger_Click(object sender, EventArgs e)
        {
            presenter.Btn_AddTrigger_Click();
        }

        private void Btn_RemoveTrigger_Click(object sender, EventArgs e)
        {
            if (dgv_Triggers.SelectedCells.Count <= 0)
            {
                Dgv_CurrentRow = 0;
                return;
            }

            Dgv_CurrentRow = dgv_Triggers.SelectedCells[0].RowIndex - 1 < 0 ? 0 : dgv_Triggers.SelectedCells[0].RowIndex - 1;
            presenter.Btn_RemoveTrigger_Click(dgv_Triggers.SelectedCells[0].Value.ToString());
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            presenter.Btn_Refresh_Click();
        }

        private void Btn_Settings_Click(object sender, EventArgs e)
        {
            presenter.Btn_Settings_Click();
        }

        private void Btn_ResetLastTriggered_Click(object sender, EventArgs e)
        {
            presenter.Btn_ResetLastTriggered_Click();
        }

        private void FormControls_ChangesMade(object sender, EventArgs e)
        {
            presenter.FormControls_ChangesMade();
        }
    }
}
