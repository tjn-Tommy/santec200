Imports System.Runtime.InteropServices


Public Class Form1
    Dim DisplayNumber As Integer = 1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim i As Integer
        Dim width, height As UShort
        Dim name As New System.Text.StringBuilder(128)
        Dim ret As Int32

        For i = 1 To 8
            ret = SLM_Disp_Info2(i, width, height, name)
            If (ret = SLM_STATUS.SLM_OK) Then
                Dim dat() As String
                If (name.ToString().IndexOf("LCOS-SLM") >= 0) Then
                    DisplayNumber = i
                    Exit For
                End If
            End If
        Next

        ret = SLM_Disp_Open(DisplayNumber)
        If (ret <> SLM_STATUS.SLM_OK) Then
            MsgBox(GetSLMError(ret))

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim gs As Integer
        Dim Flags As UInteger = 0

        If (chkRate120.Checked) Then
            Flags += SLM_FLAGS.FLAGS_RATE120

        End If
        For gs = 0 To 1000 Step 100
            Dim ret = SLM_Disp_GrayScale(DisplayNumber, Flags, gs)
            If (ret <> SLM_STATUS.SLM_OK) Then
                MsgBox(GetSLMError(ret))

            End If
            System.Threading.Thread.Sleep(100)
        Next

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ret = SLM_Disp_Close(DisplayNumber)
        If (ret <> SLM_STATUS.SLM_OK) Then
            MsgBox(GetSLMError(ret))

        End If

    End Sub
End Class
