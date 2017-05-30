﻿' Instat-R
' Copyright (C) 2015
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License k
' along with this program.  If not, see <http://www.gnu.org/licenses/>.
Imports instat
Imports instat.Translations
Public Class dlgLabels
    Private bFirstLoad As Boolean = True
    Private bReset As Boolean = True
    Private clsViewLabels As New RFunction
    Private Sub dlgLabels_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If bFirstLoad Then
            InitialiseDialog()
            bFirstLoad = False
        End If
        If bReset Then
            SetDefaults()
        End If
        SetRCodeforControls(bReset)
        bReset = False
        autoTranslate(Me)
    End Sub

    Private Sub SetRCodeforControls(bReset As Boolean)
        SetRCode(Me, ucrBase.clsRsyntax.clsBaseFunction, bReset)
    End Sub

    Private Sub TestOKEnabled()
        If Not ucrReceiverLabels.IsEmpty() AndAlso ucrFactorLabels.IsColumnComplete(0) Then
            ucrBase.OKEnabled(True)
        Else
            ucrBase.OKEnabled(False)
        End If

    End Sub

    Private Sub SetDefaults()
        clsViewLabels = New RFunction
        ucrSelectorForLabels.Reset()
        ucrSelectorForLabels.Focus()
        clsViewLabels.SetRCommand(frmMain.clsRLink.strInstatDataObject & "$set_factor_levels")
        ucrBase.clsRsyntax.SetBaseRFunction(clsViewLabels)
    End Sub

    Private Sub InitialiseDialog()
        ucrBase.iHelpTopicID = 35
        ucrReceiverLabels.Selector = ucrSelectorForLabels
        ucrReceiverLabels.SetMeAsReceiver()

        ucrReceiverLabels.SetIncludedDataTypes({"factor"})
        ucrFactorLabels.SetReceiver(ucrReceiverLabels)
        ucrFactorLabels.SetAsViewerOnly()
        ucrFactorLabels.AddEditableColumns({"Levels"})

        ucrFactorLabels.SetParameter(New RParameter("new_levels", 2))
        ucrReceiverLabels.SetParameter(New RParameter("col_name", 1))
        ucrReceiverLabels.SetParameterIsString()

        ucrSelectorForLabels.SetParameter(New RParameter("data_name", 0))
        ucrSelectorForLabels.SetParameterIsString()
    End Sub

    Private Sub ucrBase_ClickReset(sender As Object, e As EventArgs) Handles ucrBase.ClickReset
        SetDefaults()
        SetRCodeforControls(True)
        TestOKEnabled()
    End Sub

    Private Sub cmdAddLevel_Click(sender As Object, e As EventArgs) Handles cmdAddLevel.Click
        ucrFactorLabels.AddLevel()
        TestOKEnabled()
    End Sub

    Private Sub ucrReceiverLabels_ControlContentsChanged(ucrChangedControl As ucrCore) Handles ucrReceiverLabels.ControlContentsChanged, ucrFactorLabels.ControlContentsChanged
        cmdAddLevel.Enabled = ucrFactorLabels.grdFactorData.Visible
        TestOKEnabled()
    End Sub
End Class