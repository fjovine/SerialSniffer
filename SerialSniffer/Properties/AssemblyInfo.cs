using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Le informazioni generali relative a un assembly sono controllate dal seguente 
// set di attributi. Modificare i valori di questi attributi per modificare le informazioni
// associate a un assembly.
[assembly: AssemblyTitle("SerialSniffer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SerialSniffer")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Se si imposta ComVisible su false, i tipi in questo assembly non saranno visibili 
// ai componenti COM. Se è necessario accedere a un tipo in questo assembly da 
// COM, impostare su true l'attributo ComVisible per tale tipo.
[assembly: ComVisible(false)]

// Se il progetto viene esposto a COM, il GUID seguente verrà utilizzato come ID della libreria dei tipi
[assembly: Guid("bff257ab-9824-4324-acef-49cb63f03457")]

// TODO
// v Attivare il parametro collapsed da linea di comando
// - memorizzare i parameteri sul registry
// v Eliminare il problema che non esce chiudendo la finestra
// - In modalità GUI l'output su file deve essere a comando
//
// Le informazioni sulla versione di un assembly sono costituite dai seguenti quattro valori:
//
//      Versione principale
//      Versione secondaria 
//      Numero di build
//      Revisione
//
// È possibile specificare tutti i valori oppure impostare valori predefiniti per i numeri relativi alla revisione e alla build 
// usando l'asterisco '*' come illustrato di seguito:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.103")]
[assembly: AssemblyFileVersion("1.0.0.103")]

//// 1.0.0.103  Compiled 22.09.2021 22:22:25

//// 1.0.0.102  Compiled 22.09.2021 22:21:11

//// 1.0.0.101  Compiled 25.02.2020 08:08:46

//// 1.0.0.100  Compiled 11/11/2017 17.52.25

//// 1.0.0.99  Compiled 18/08/2017 21:48:56

//// 1.0.0.98  Compiled 8/16/2017 10:16:52 PM

//// 1.0.0.97  Compiled 11/08/2017 16.04.54
//// When only ascii, newline interrupts the flow of output

//// 1.0.0.96  Compiled 11/08/2017 15.54.55

//// 1.0.0.95  Compiled 11/08/2017 15.46.27

//// 1.0.0.94  Compiled 11/08/2017 15.44.05

//// 1.0.0.93  Compiled 11/08/2017 15.39.03

//// 1.0.0.92  Compiled 11/08/2017 15.36.58

//// 1.0.0.91  Compiled 11/08/2017 15.35.39

//// 1.0.0.90  Compiled 11/08/2017 15.33.41

//// 1.0.0.89  Compiled 11/08/2017 15.28.09

//// 1.0.0.88  Compiled 11/08/2017 15.26.42
//// When in collapsed, a timeout of 0.1 seconds before emitting

//// 1.0.0.87  Compiled 11.08.2017 13:38:00
//// Added NONE option as -tx to transform the sniffer in a terminal reader

//// 1.0.0.86  Compiled 11.08.2017 13:24:11

//// 1.0.0.85  Compiled 04.01.2017 10:29:50

//// 1.0.0.84  Compiled 04.01.2017 10:28:38
//// Added Mode combo box to GUI

//// 1.0.0.83  Compiled 04.01.2017 10:22:08

//// 1.0.0.82  Compiled 04.01.2017 10:20:22

//// 1.0.0.81  Compiled 04.01.2017 10:04:33

//// 1.0.0.80  Compiled 04.01.2017 10:02:19

//// 1.0.0.79  Compiled 04.01.2017 09:59:35

//// 1.0.0.78  Compiled 04.01.2017 09:55:17

//// 1.0.0.77  Compiled 04.01.2017 09:53:22

//// 1.0.0.76  Compiled 04.01.2017 09:48:16

//// 1.0.0.75  Compiled 04.01.2017 09:07:52
//// Added support for Y Cable mode

//// 1.0.0.74  Compiled 14.10.2016 07:51:06

//// 1.0.0.73  Compiled 29.06.2016 08:53:09

//// 1.0.0.72  Compiled 29.06.2016 08:52:59

//// 1.0.0.71  Compiled 28.06.2016 16:21:53

//// 1.0.0.70  Compiled 07/04/2016 19.18.07

//// 1.0.0.69  Compiled 07/04/2016 18.42.53

//// 1.0.0.68  Compiled 07/04/2016 18.42.35

//// 1.0.0.67  Compiled 07/04/2016 18.38.21

//// 1.0.0.66  Compiled 07/04/2016 18.37.57

//// 1.0.0.65  Compiled 07/04/2016 18.37.41

//// 1.0.0.64  Compiled 07/04/2016 18.35.48

//// 1.0.0.63  Compiled 07/04/2016 18.35.14

//// 1.0.0.62  Compiled 07/04/2016 18.25.28

//// 1.0.0.61  Compiled 07/04/2016 18.21.20

//// 1.0.0.60  Compiled 07/04/2016 18.20.05

//// 1.0.0.59  Compiled 07/04/2016 18.17.26

//// 1.0.0.58  Compiled 07/04/2016 18.16.49

//// 1.0.0.57  Compiled 07/04/2016 06.53.25

//// 1.0.0.56  Compiled 07/04/2016 06.48.45

//// 1.0.0.55  Compiled 07/04/2016 06.47.57

//// 1.0.0.54  Compiled 07/04/2016 06.39.29

//// 1.0.0.53  Compiled 07/04/2016 06.26.08

//// 1.0.0.52  Compiled 07/04/2016 06.22.34

//// 1.0.0.51  Compiled 07/04/2016 06.12.16
//// Added Registry persistence in gui mode

//// 1.0.0.50  Compiled 04/04/2016 06.53.37

//// 1.0.0.49  Compiled 04/04/2016 06.50.59

//// 1.0.0.48  Compiled 04/04/2016 06.45.03

//// 1.0.0.47  Compiled 04/04/2016 06.42.08

//// 1.0.0.46  Compiled 04/04/2016 06.40.01
//// Retrivial of the GUI programmed parameters

//// 1.0.0.45  Compiled 04/04/2016 06.27.56

//// 1.0.0.44  Compiled 04/04/2016 06.26.25

//// 1.0.0.43  Compiled 04/04/2016 06.19.58

//// 1.0.0.42  Compiled 04/04/2016 06.15.54
//// Hidden console window at startup in GUI mode

//// 1.0.0.41  Compiled 04/04/2016 06.03.15

//// 1.0.0.40  Compiled 04/04/2016 06.01.26
//// Modified the interaction: now the save button saves the sniffed data only on demand

//// 1.0.0.39  Compiled 04/04/2016 05.55.08

//// 1.0.0.38  Compiled 04/04/2016 05.54.20

//// 1.0.0.37  Compiled 04/04/2016 05.53.36
//// Output file on GUI is on demand

//// 1.0.0.36  Compiled 04/04/2016 05.50.48

//// 1.0.0.35  Compiled 04/04/2016 05.39.11

//// 1.0.0.34  Compiled 04/04/2016 05.36.25

//// 1.0.0.33  Compiled 04/04/2016 05.34.58

//// 1.0.0.32  Compiled 04/04/2016 05.34.15
//// GUI: check valididty of port fields

//// 1.0.0.31  Compiled 04/04/2016 05.13.50

//// 1.0.0.30  Compiled 4/3/2016 9:42:51 AM

//// 1.0.0.29  Compiled 02/04/2016 19.01.01

//// 1.0.0.28  Compiled 01/04/2016 19.51.36

//// 1.0.0.27  Compiled 01/04/2016 19.20.39

//// 1.0.0.26  Compiled 01/04/2016 19.14.45

//// 1.0.0.25  Compiled 01/04/2016 19.14.24

//// 1.0.0.24  Compiled 01/04/2016 19.10.57

//// 1.0.0.23  Compiled 01/04/2016 19.09.41

//// 1.0.0.22  Compiled 01/04/2016 19.07.40

//// 1.0.0.21  Compiled 01/04/2016 19.07.11
//// Added the UniqueParameters concept to the command line parsing  classes
//// Now it is possible to use -gui and -help alone

//// 1.0.0.20  Compiled 01/04/2016 18.46.31

//// 1.0.0.19  Compiled 01/04/2016 18.39.28

//// 1.0.0.18  Compiled 01/04/2016 18.38.19
//// Implemeted exception management when the COM port is closed

//// 1.0.0.17  Compiled 01/04/2016 06.52.10

//// 1.0.0.16  Compiled 01/04/2016 06.48.29

//// 1.0.0.15  Compiled 01/04/2016 06.47.02

//// 1.0.0.14  Compiled 01/04/2016 06.17.58

//// 1.0.0.13  Compiled 01/04/2016 06.16.16
//// Sync between command lines and GUI widgets

//// 1.0.0.12  Compiled 01/04/2016 06.10.42
//// Now Closes when gui dialog is closed

//// 1.0.0.11  Compiled 01/04/2016 06.04.53

//// 1.0.0.10  Compiled 01/04/2016 06.03.05

//// 1.0.0.9  Compiled 01/04/2016 06.00.16

//// 1.0.0.8  Compiled 01/04/2016 05.59.20

//// 1.0.0.7  Compiled 01/04/2016 05.59.04

//// 1.0.0.6  Compiled 01/04/2016 05.55.51

//// 1.0.0.5  Compiled 01/04/2016 05.53.26
//// Support for -collapsed command line option

//// 1.0.0.4  Compiled 01/04/2016 05.40.23

//// 1.0.0.3  Compiled 01/04/2016 05.35.10

//// 1.0.0.2  Compiled 31/03/2016 22.33.36

//// 1.0.0.1  Compiled 31/03/2016 22.29.14
//// Prima versione operativa con gui funzionante bicolore
