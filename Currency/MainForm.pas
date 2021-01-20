unit MainForm;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, StrUtils;

type
  TForm1 = class(TForm)
    IntegerInputCaption: TLabel;
    IntegerInput: TEdit;
    IntegerOutput: TLabel;
    function IntegerUpToHundred(UpToHundred: integer; Row: integer): string;
    function Convert(Value: Int64): string;
    procedure IntegerInputChange(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

  Output: string;
  Digits: array[0..9] of string = ('nule', 'viens', 'divi', 'trīs', 'četri', 'pieci', 'seši', 'septiņi', 'astoņi', 'deviņi' );
  DecimalsSmall: array[0..9] of string = ('desmit', 'vienpadsmit', 'divpadsmit', 'trīspadsmit', 'četrpadsmit', 'piecpadsmit', 'sešpadsmit', 'septiņpadsmit', 'astoņpadsmit', 'deviņpadsmit');
  DecimalsBig: array[0..9] of string = ('', 'desmit', 'divdesmit', 'trīsdesmit', 'četrdesmit', 'piecdesmit', 'sešdesmit', 'septiņdesmit', 'astoņdesmit', 'deviņdesmit' );
  Hundreds: array[0..2] of string = ('', 'simts', 'simti');
  Thousands: array[0..2] of string = ('', 'tukstotis', 'tukstoši');
  Millions: array[0..2] of string = ('', 'miljons', 'miljoni');
  Milliards: array[0..2] of string = ('', 'miljards', 'miljardi');
  Trillions: array[0..2] of string = ('', 'trilions', 'trilioni');
  Kvadrilions: array[0..2] of string = ('', 'kvadrilions', 'kvadrilioni');
  Kvintilions: array[0..2] of string = ('', 'kvintilions', 'kvintilioni');

implementation

{$R *.dfm}

procedure TForm1.IntegerInputChange(Sender: TObject);
var Value: Extended;
    IntegerPart: Int64;
    DecimalPart: integer;
    IntegerPartString, DecimalPartString: string;
begin

  IntegerOutput.Caption := 'Maksimālais skaitlis, kuru var apstradāt ir Int64.' + #13#10 + #13#10;
  if (Length(IntegerInput.Text) > 0) then
  begin

    Value := StrToFloat(IntegerInput.Text);

    IntegerPart := Trunc(Value * 1.0);
    DecimalPart := Round(Frac(Value) * 100);

    IntegerPartString := Convert(IntegerPart);
    DecimalPartString := Convert(DecimalPart);

    case DecimalPart of
      1, 21, 31, 41, 51, 61, 71, 81, 91:
        IntegerOutput.Caption := IntegerOutput.Caption + 'Rezultāts:' + #13#10 + IntegerPartString + ' eiro un ' + DecimalPartString + ' cents.';
    else
      IntegerOutput.Caption := IntegerOutput.Caption + 'Rezultāts:' + #13#10 +  IntegerPartString + ' eiro un ' + DecimalPartString + ' centi.';
    end;


  end
  else
    ShowMessage('Skaitlim jābūt lielākam par nulli un mazākam par Int64 - ' + IntToStr(Int64.MaxValue));
end;

// Перевод чисел, которые меньше тысячи
function TForm1.IntegerUpToHundred(UpToHundred: integer; Row: integer): string;
var DigitsCount, DecimalsCount, HundredsCount: integer;
    Output: string;
begin
  DigitsCount := UpToHundred mod 10;
  DecimalsCount := (UpToHundred div 10) mod 10;
  HundredsCount := (UpToHundred div 100) mod 10;

  if HundredsCount = 1 then
    Output := Digits[HundredsCount] + ' ' + Hundreds[1] + ' ';
  if HundredsCount > 1 then
    Output := Digits[HundredsCount] + ' ' + Hundreds[2] + ' ';

  if DecimalsCount = 1 then
    Output := Output + DecimalsSmall[DigitsCount]
  else
    if not (DigitsCount = 0) then
      Output := Output + DecimalsBig[DecimalsCount] + ' ' + Digits[DigitsCount]
    else
      Output := Output + DecimalsBig[DecimalsCount] + ' ';

  if (Row = 1) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Thousands[1] + ' '
    else
      Output := Output + ' ' + Thousands[2] + ' ';

  if (Row = 2) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Millions[1] + ' '
    else
      Output := Output + ' ' + Millions[2] + ' ';

   if (Row = 3) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Milliards[1] + ' '
    else
      Output := Output + ' ' + Milliards[2] + ' ';

   if (Row = 4) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Trillions[1] + ' '
    else
      Output := Output + ' ' + Trillions[2] + ' ';

   if (Row = 5) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Kvadrilions[1] + ' '
    else
      Output := Output + ' ' + Kvadrilions[2] + ' ';

   if (Row = 6) and ((DigitsCount > 0) or (DecimalsCount > 0) or (HundredsCount > 0)) then
    if DigitsCount = 1 then
      Output := Output + ' ' + Kvintilions[1] + ' '
    else
      Output := Output + ' ' + Kvintilions[2] + ' ';

   // и так можно продолжать до бесконечности (хотя бы до дециллионов)

  IntegerUpToHundred := Output;
end;

// Основная часть конвертера
function TForm1.Convert(Value: Int64): string;
var Output: string;
    i, j, k, n: integer;
    UpToHundred: integer;
    tmp: string;
    Res: array[1..10] of string;
begin

  if Value = 0 then
    Output := Digits[0]
  else
  begin
    // Немного запутанная, но работающая логика

    // Этап первый: реверс строки
    // Этап второй: высчет количества сотен разрядов (n)
    // Этап третий: перевести каждую сотню в текст + название системы наименования => запись в массив
    // Этап четвёртый: реверс массива и вывод string

    tmp := ReverseString(IntToStr(Value));
    i := Length(Value.ToString());
    n := (i - 1) div 3;
    j := 1;

    for k := 0 to n do
    begin
      UpToHundred := StrToInt(ReverseString(Copy(tmp, j, 3)));
      Res[k + 1] := IntegerUpToHundred(UpToHundred, k);
      j := j + 3;
    end;

    for i := Length(res) downto 1 do
    begin
      Output := Output + res[i];
    end;
  end;

  Result := Output;
end;

end.
