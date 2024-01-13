using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ShapesNumbersModule : BaseModule
{
    private Shape shape;
    public List<int> buttonNumberList;

    private enum Shape
    {
        Square=0,
        Triangle=1,
        Circle=2,
    }

    public override void GenerateProblem()
    {
        // pertama tentuin dulu shapesnya apa
        // terus dari shape kan dia nanti ada masing" kasus
        // dari kasus" itu, baru kita bikin value button"nya

        // tentuin shape yang muncul
        Shape[] shapesEnum = (Shape[])Enum.GetValues(typeof(Shape));
        shape = shapesEnum[Random.Range(0, shapesEnum.Length)];

        // reset buttonNumberList-nya
        buttonNumberList.Clear();

        // habis itu, cek dia itu kasusnya apa
        // anyway, untuk peletakan angka didalam buttonNumberList, 3 angka pertama itu pasti yang dihighlight
        switch (shape)
        {
            case Shape.Square:
                {
                    // Tekan 3 tombol yang jika dijumlahkan, akan menghasilkan nilai lebih dari 3 tombol yang terhighlight jika dijumlahkan
                    // kalau ini, berarti tentuin dulu 3 nilai yang mau dihighlight itu brp, tbh, isn't this actually pretty easy for the kids...? whatever
                    buttonNumberList.Add(Random.Range(1, 10));
                    buttonNumberList.Add(Random.Range(1, 5));
                    buttonNumberList.Add(Random.Range(1, 3));

                    // jadi 3 angka yang mw dihighlight udah ditentuin, which is 3 angka pertama didalam listnya
                    // itu udh ditentuin nilainya, sekarang tinggal bikin nomor yang lain, which is sisa 9
                    for (int i = 0; i < 9; i++)
                    {
                        buttonNumberList.Add(Random.Range(3, 9));
                    }
                    // i mean, gw gtw sih kalo dia ada solutionnya ato nggak, but yea, whatevs
                    // well, actually, kalo randomnya kek begini, udh pasti ada solutionnya sih, so no problem there
                    break;
                }
            case Shape.Triangle:
                {
                    // 2 tombol yang jika dikalikan, menghasilkan nilai sesuai dengan 3 tombol yang terhighlight jika dijumlahkan
                    // kalo ini, tentuin dulu 2 tombol yang perlu dikaliin
                    // cause penjumlahan is easy to do, but perkalian lebih shitty bwt disamain
                    // harusnya bisa lah ya mereka perkalian 2 sampe 10...
                    int num1 = Random.Range(2, 11);
                    int num2 = Random.Range(2, 11);

                    // ini udah dapet 2 angkanya, sekarang tinggal tinggal tambahin nilai spesifiknya...
                    // again, i didn't think this through pas mikirin game designnya, ini bukannya nnti angkanya bisa jomplang ya...? kayak 10 terus next numbernya ada 100.....
                    // oh well
                    int a = Random.Range(1, num1 * num2 - 2);
                    int b = Random.Range(1, num1 * num2 - a);
                    int c = num1 * num2 - a - b;

                    buttonNumberList.Add(a);
                    buttonNumberList.Add(b);
                    buttonNumberList.Add(c);
                    buttonNumberList.Add(num1);
                    buttonNumberList.Add(num2);

                    // udah ada 5 angka, sekarang tinggal pasang 7 angka lain
                    for (int i = 0; i < 7; i++)
                    {
                        buttonNumberList.Add(Random.Range(2, 11));
                    }
                    break;
                }
            case Shape.Circle:
                {
                    // Tekan 3 tombol yang jika dijumlahkan, merupakan angka yang memiliki nilai faktor dari salah satu tombol yang terhighlight
                    // wait what? sbr, so... uh... jadi kalo misalkan 3 tombol ini dijumlahin, dia merupakan salah satu faktor dari tombol yang terhighlight...?
                    // well kalo gitu cara yang paling gampang itu ya tentuin nilai dari salah satu tombol yang terhighlight, terus dikaliin sama nilai random, terus pisahin jadi 3 angka yang kalo dijumlahin bakalan senilai sama nilai yang dikaliin sama nilai random
                    // terus sisanya bisa masukin angka random, yang dimana bisa jadi jawabannya ada lebih dari 1 solusi

                    // tentuin dulu nilai dari salah satu tombol yang terhighlight, which means nilai faktornya
                    // well, berhubung ini buat anak kelas 2 sampe 3, nilainya gw pasang 1 - 10 aja
                    int factor1 = Random.Range(1, 11);
                    int result = factor1 * Random.Range(3, 11);

                    int a = Random.Range(1, result - 1);
                    int b = Random.Range(1, result - a);
                    int c = result - a - b;

                    int factor2 = Random.Range(1, 11);
                    int factor3 = Random.Range(1, 11);

                    buttonNumberList.Add(factor1);
                    buttonNumberList.Add(factor2);
                    buttonNumberList.Add(factor3);
                    buttonNumberList.Add(a);
                    buttonNumberList.Add(b);
                    buttonNumberList.Add(c);
                    for (int i = 0; i < 6; i++)
                    {
                        buttonNumberList.Add(Random.Range(1, 99));
                    }

                    break;
                }
        }
    }

    public override void GenerateSolution()
    {
        switch (shape)
        {
            case Shape.Square:
                solution = buttonNumberList[0] + buttonNumberList[1] + buttonNumberList[2];
                break;
            case Shape.Triangle:
                solution = buttonNumberList[0] + buttonNumberList[1] + buttonNumberList[2];
                break;
            case Shape.Circle:
                solution = new int[] { buttonNumberList[0], buttonNumberList[1], buttonNumberList[2] };
                break;
        }
    }

    public override bool CheckAnswer(object answer)
    {
        // well, object answer itu nggak selalu ada 3 jawaban, so it really depends
        // tapi apa yang pasti itu adalah dia pasti IEnumerable<int>, jadi mau bagaimana pun, dia pasti bisa dicast jadi itu

        List<int> castedAnswer = new List<int>((IEnumerable<int>)answer);

        switch (shape)
        {
            case Shape.Square:
                {
                    return (castedAnswer[0] + castedAnswer[1] + castedAnswer[2]) > (int)solution;
                }
            case Shape.Triangle:
                {
                    return (castedAnswer[0] * castedAnswer[1]) == (int)solution;
                }

            case Shape.Circle:
                {
                    int totalAnswer = castedAnswer[0] + castedAnswer[1] + castedAnswer[2];
                    bool isCorrect = false;
                    foreach (int factor in (IEnumerable<int>)solution)
                    {
                        isCorrect = (totalAnswer % factor == 0) || isCorrect;
                    }
                    return isCorrect;
                }
            default:
                Debug.LogWarning("How did you get here...? Shape: " + shape.ToString());
                return false;
        }
    }
}
