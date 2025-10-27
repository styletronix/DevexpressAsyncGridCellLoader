''' <summary>
''' Copyright (C) 2025 by Andreas W. Pross (Styletronix.net GmbH)
''' 
''' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal
''' in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
''' of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
''' 
''' The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
''' 
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
''' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
''' WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </summary>
Public Class StubGlyph
    Public Shared Function GenerateStubGlyph(Glyph As String, Size As Size, BackgroundColor As Color, TextColor As Color, CornerRadius As Integer) As Bitmap
        Dim rect = New Drawing.Rectangle(0, 0, Size.Width, Size.Height)
        Dim dat = New Drawing.Bitmap(rect.Width, rect.Height)

        Using g As Drawing.Graphics = Drawing.Graphics.FromImage(dat)
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit

            Dim TextRect As New RectangleF(0, 0, rect.Width, rect.Height)

            Using path = RoundedRect(rect, CornerRadius)
                Using p As New Pen(BackgroundColor, 0)
                    g.DrawPath(p, path)
                End Using
                Using b As New System.Drawing.SolidBrush(BackgroundColor)
                    g.FillPath(b, path)
                End Using
                Using f As New Font(FontFamily.GenericSansSerif, rect.Height, FontStyle.Bold)
                    Dim RealSize = g.MeasureString(Glyph, f)
                    Dim RatioWidth = TextRect.Width / RealSize.Width
                    Dim RatioHeight = TextRect.Height / RealSize.Height
                    Dim ScaleRatio As Double
                    If RatioHeight < RatioWidth Then
                        ScaleRatio = RatioHeight
                    Else
                        ScaleRatio = RatioWidth
                    End If

                    Using f2 As New Font(f.FontFamily, CType(f.Size * ScaleRatio, Single), FontStyle.Bold)
                        Using b As New System.Drawing.SolidBrush(TextColor)
                            g.DrawString(Glyph, f2, b, rect, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        End Using
                    End Using
                End Using
            End Using
        End Using

        Return dat
    End Function
    Private Shared Function RoundedRect(Bounds As Rectangle, radius As Integer) As Drawing2D.GraphicsPath
        Dim diameter = radius * 2
        Dim size = New Size(diameter, diameter)
        Dim arc = New Rectangle(Bounds.Location, size)
        Dim path = New Drawing2D.GraphicsPath()

        If radius = 0 Then
            path.AddRectangle(Bounds)
            Return path
        End If

        path.AddArc(arc, 180, 90)

        arc.X = Bounds.Right - diameter
        path.AddArc(arc, 270, 90)

        arc.Y = Bounds.Bottom - diameter
        path.AddArc(arc, 0, 90)

        arc.X = Bounds.Left
        path.AddArc(arc, 90, 90)

        path.CloseFigure()
        Return path
    End Function
End Class
