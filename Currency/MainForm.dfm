object Form1: TForm1
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  Caption = 'Skait'#316'i ar burtiem'
  ClientHeight = 197
  ClientWidth = 440
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object IntegerInputCaption: TLabel
    Left = 8
    Top = 8
    Width = 73
    Height = 13
    Caption = 'Ievadiet skaitli:'
  end
  object IntegerOutput: TLabel
    Left = 8
    Top = 32
    Width = 424
    Height = 157
    AutoSize = False
    Caption = 'Maksim'#257'lais skaitlis, kuru var apstrad'#257't ir Int64.'
    WordWrap = True
  end
  object IntegerInput: TEdit
    Left = 87
    Top = 5
    Width = 345
    Height = 21
    TabOrder = 0
    OnChange = IntegerInputChange
  end
end
