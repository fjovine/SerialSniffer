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
[assembly: AssemblyVersion("1.0.0.28")]
[assembly: AssemblyFileVersion("1.0.0.28")]

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
