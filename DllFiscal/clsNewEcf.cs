﻿/* --------------------------------------------------------------------------------------------------------------------
Fernando Passaia - https://www.linkedin.com/pub/fernando-passaia/24/622/285 - https://www.facebook.com/fernando.passaia
Blog: fernandopassaia.wordpress.com - Email/Skype: fernandopassaia@futuradata.com.br - Cel/Whatsapp: (11)98104-9080
Para feedbacks - favor utilizar o GitHub - ou enviar através dos contatos acima.

Classe de interface para ECFs (impressoras fiscais). Essa classe possui métodos para comunicação com ECF da Bematech,
Daruma, Epson e Sweda. Nota: Você deve inserir as DLLs dos fabricantes na pasta System32 (ou SysWON64) conforme
instruções do fabricante do ECF. As classes de "interface" foram fornecidas e/ou retiradas de exemplos dos fabricantes.
Essa classe em geral foi programada entre 2008 e 2010 e funciona perfeitamente - mas precisa de revisões/melhorias.

 * Nota: Todos podem colaborar subindo suas melhorias, novos métodos e correções para esse projeto totalmente Opensource
 * e livre para uso de quem quiser em qualquer tipo de aplicação. Nota2: Por padrão, compila em C:\CSharp_BasicFramework
 * Caso o diretório não exista - efetue sua criação antes de abrir esse projeto e efetuar o Build.
--------------------------------------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using DllValidacoes;
using System.Windows.Forms;
using DllCrypt;

namespace DllFiscal
{
    public class clsNewEcf
    {
        #region Váriaveis Internas e Construtor da Classe
        private int IRetornoBematech;
        private int IRetornoDaruma;
        private int IRetornoSweda;
        private int IRetornoEpson;        
        #endregion

        #region Métodos referentes a Gerenciamento, Configuração, Relatórios e Totalizadores dos ECFs (ok Rev2)

        #region Verifica Impressora Fiscal Ligada (ok rev2)
        /// <summary>
        /// Verifica se Impressora Fiscal está ligada
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna "OK" se estiver, "ERRO" se não</returns>
        public string verificaEstadoImpressora(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {               
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_VerificaImpressoraLigada();
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                if (IRetornoBematech == 1)
                {
                    return "OK";
                }
                else
                {
                    return "ERRO";
                }
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                if (IRetornoDaruma != 1)
                {
                    MessageBox.Show("Impressora desligada");
                }
                clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                                
                StringBuilder Str_Status = new StringBuilder(15);
                //DeclaracoesDaruma.iRetorno = DeclaracoesDaruma.rStatusImpressora_ECF_Daruma(Str_Status);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rVerificarImpressoraLigada_ECF_Daruma();// rStatusImpressora_ECF_Daruma(Str_Status);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                if (IRetornoDaruma == 1)
                {
                    return "OK";
                }
                else
                {
                    return "ERRO";
                }
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                IRetornoEpson = clsInterfaceEpson.EPSON_Serial_Abrir_Porta(115200, 1);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Serial_Abrir_Porta");

                if (IRetornoEpson == 0)
                {
                    return "OK";
                }
                else
                {
                    return "ERRO";
                }               
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_VerificaImpressoraLigada();
                //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                if (IRetornoSweda == 1)
                {
                    return "OK";
                }
                else
                {
                    return "ERRO";
                }
            }
            return "ERRO";
        }
        #endregion

        #region Emite Relatório Gerencial (ok rev2)
        /// <summary>
        /// Emite informação em relatório gerencial no ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <param name="mensagemRelatorioGerencial">Mensagem para Ser impressa no relatório gerencial (texto livre até 580 caracters)</param>
        /// <returns>Retorna True se enviar comando, false se não...</returns>
        public bool emiteRelatorioGerencial(string modeloImpressora, string mensagemRelatorioGerencial)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_RelatorioGerencial("Relatório Gerencial FuturaData" + DateTime.Now.ToString() + " - " + mensagemRelatorioGerencial);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);


                IRetornoBematech = clsInterfaceBematech.Bematech_FI_FechaRelatorioGerencial();
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.iRGAbrir_ECF_Daruma("Relatório Gerencial FuturaData" + DateTime.Now.ToString() + " - " + mensagemRelatorioGerencial);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.iRGFechar_ECF_Daruma();
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                IRetornoEpson = clsInterfaceEpson.EPSON_NaoFiscal_Abrir_Relatorio_Gerencial("Relatório Gerencial FuturaData" + DateTime.Now.ToString() + " - " + mensagemRelatorioGerencial);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_NaoFiscal_Abrir_Relatorio_Gerencial");

                IRetornoEpson = clsInterfaceEpson.EPSON_NaoFiscal_Fechar_Relatorio_Gerencial(false);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_NaoFiscal_Fechar_Relatorio_Gerencial");
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_AbreRelatorioGerencial();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();
                
                try
                {
                    IRetornoSweda = clsInterfaceSweda.ECF_EnviarTextoCNF("Relatório Gerencial FuturaData" + DateTime.Now.ToString() + " - " + mensagemRelatorioGerencial);
                }
                catch (Exception ex) { MessageBox.Show("String Maior que 618 Caracter", ex.Message); }


                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();

                IRetornoSweda = clsInterfaceSweda.ECF_FechaRelatorioGerencial();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();                
            }
            return true;
        }
        #endregion

        #region Leitura da Memória Fiscal por Data (ok rev2)
        /// <summary>
        /// Leitura da Memória Fiscal por Data (o próprio método seleciona a data inteira, de 2003 até a atual)
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna True se enviar o comando corretamente, false se não</returns>
        public bool leituraMemoriaFiscalPorData(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_LeituraMemoriaFiscalData("01/12/2003", DateTime.Now.ToString("dd/MM/yyyy"));
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.regAlterarValor_Daruma(@"ECF\LMFCompleta", "1");
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.iMFLer_ECF_Daruma(Str_Parametro_Inicial.Replace("/", ""), Str_Parametro_Final.Replace("/", ""));
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                int tambuff = 0x00;
                string pszDados = new String(' ', 1023);
                IRetornoEpson = clsInterfaceEpson.EPSON_RelatorioFiscal_Leitura_MF(Str_Parametro_Inicial.Replace("/", ""), Str_Parametro_Final.Replace("/", ""), 4, ref pszDados, "C:\\FuturaData\\epson.txt", tambuff, 1024);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_RelatorioFiscal_Leitura_MF");
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                IRetornoSweda = clsInterfaceSweda.ECF_LeituraMemoriaFiscalData(Str_Parametro_Inicial, Str_Parametro_Final);
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();
            }
            return true;
        }
        #endregion

        #region Leitura da Memória Fiscal por Redução (ok rev2)
        /// <summary>
        /// Efetua a Leitura da Memória Fiscal por Redução
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna True se der OK, false se não</returns>
        public bool leituraMemoriaFiscalPorReducao(string modeloImpressora)
        {

            //ATENÇÃO: ESSE MÉTODO NÃO FOI FEITO POR QUE NO SISTEMA ESTÁ APENAS A LEITURA POR DATA COMPLETA... FERNANDO 13/07/2013
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_LeituraMemoriaFiscalReducao("01/12/2003", DateTime.Now.ToString("dd/MM/yyyy"));
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.regAlterarValor_Daruma(@"ECF\LMFCompleta", "1");
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.iMFLer_ECF_Daruma(Str_Parametro_Inicial.Replace("/", ""), Str_Parametro_Final.Replace("/", ""));
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                int tambuff = 0x00;
                string pszDados = new String(' ', 1023);
                IRetornoEpson = clsInterfaceEpson.EPSON_RelatorioFiscal_Leitura_MF(Str_Parametro_Inicial.Replace("/", ""), Str_Parametro_Final.Replace("/", ""), 4, ref pszDados, "C:\\FuturaData\\epson.txt", tambuff, 1024);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_RelatorioFiscal_Leitura_MF");
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                string Str_Parametro_Inicial = "01/12/2003";
                string Str_Parametro_Final = DateTime.Now.ToString("dd/MM/yyyy");

                IRetornoSweda = clsInterfaceSweda.ECF_LeituraMemoriaFiscalData(Str_Parametro_Inicial, Str_Parametro_Final);
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();
            }
            return true;            
        }
        #endregion

        #region Retorna CNPJ, IE e Clichê do ECF (ok rev2)
        /// <summary>
        /// Retorna CNPJ, Inscrição Estadual e Cliche do ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna as Informações</returns>
        public string retornaCNPJIEClicheDoECF(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string CGC = new string('\x20', 18);
                string IE = new string('\x20', 15);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_CGC_IE(ref CGC, ref IE);
                string Cliche = new string('\x20', 186);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_ClicheProprietario(ref Cliche);

                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                string retorno = "CNPJ: " + CGC + ". I.E: " + IE + ". Cliche: " + Cliche;
                return retorno;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                

                string Str_indice;
                StringBuilder Str_Razao = new StringBuilder(50);
                StringBuilder Str_CNPJ = new StringBuilder(20);
                StringBuilder Str_Cliche = new StringBuilder(219);
                                
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("87", Str_Razao);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("90", Str_CNPJ);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("132", Str_Cliche);

                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);

                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return "Razão: " + Str_Razao.ToString() + ". CNPJ: " + Str_CNPJ.ToString() + ". Chichê: " + Str_Cliche.ToString();
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szDadosUsuario = new String(' ', 48);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Dados_Usuario(ref szDadosUsuario);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Dados_Usuario");

                string szDadosImpressora = new String(' ', 108);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Dados_Impressora(ref szDadosImpressora);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Dados_Impressora");

                string szUsuario = new String(' ', 160);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Cliche_Usuario(ref szUsuario);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Cliche_Usuario");

                if (IRetornoEpson == 0x00)
                    return "Dados do usuário: " + szDadosUsuario + ". Dados da Impressora: " + szDadosImpressora + ". Dados Clichê: " + szUsuario;
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                return "Desculpe mas não foi localizado nenhum método em sua ECF Sweda para obter o CNPJ/Usuário/Clichê Atual do ECF";
            }
            return "";
        }
        #endregion

        #region Retorna Data e Hora da Impressora (ok rev2)
        /// <summary>
        /// Retorna Data e Hora da Impressora Fiscal
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna a Data e Hora do ECF</returns>
        public string retornaDataEHoraDaImpressora(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string Data = new string('\x20', 6);
                string Hora = new string('\x20', 6);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_DataHoraImpressora(ref Data, ref Hora);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return Data + "-" + Hora;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_Data = new StringBuilder(10);
                StringBuilder Str_Hora = new StringBuilder(10);
                
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rDataHoraImpressora_ECF_Daruma(Str_Data, Str_Hora);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return "Data: " + Str_Data.ToString().Trim() + " Hora: " + Str_Hora.ToString().Trim();
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szDados = new String(' ', 14);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Hora_Relogio(ref szDados);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Hora_Relogio");
                if (IRetornoEpson == 0x00)
                    return "Data e Hora do ECF: " + szDados;
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                return "Desculpe mas não foi localizado nenhum método em sua ECF Sweda para obter a Data/Hora Atual do ECF";
            }
            return "";
        }
        #endregion

        #region Retorna Numero do Ultimo CCO do ECF (ok rev2)
        /// <summary>
        /// Retorna o Numero do Ultimo CCO no ECF MFD
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna o número do Ultimo CCO do ECF</returns>
        public string retornaNumeroCOO(string impressora)
        {            
            if(impressora.ToUpper() == "BEMATECH")
            {
                string cco = new string('\x20', 14);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_NumeroCupom(ref cco);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return cco;                   
            }

            if (impressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_CCo = new StringBuilder(10);                

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("26", Str_CCo);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                //IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                if (Str_CCo.Length > 6)
                {
                    return Str_CCo.ToString().Substring(0, 6);
                }
                return Str_CCo.ToString();
            }

            if (impressora.ToUpper() == "EPSON")
            {
                string szInfo = new String(' ', 30);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Informacao_Ultimo_Documento(ref szInfo);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Informacao_Ultimo_Documento");
                if (IRetornoEpson == 0x00)
                    return szInfo;
            }

            if (impressora.ToUpper() == "SWEDA")
            {
                StringBuilder RetornaCOO = new StringBuilder("      ");
                IRetornoSweda = clsInterfaceSweda.ECF_RetornaCOO(RetornaCOO);
                //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);

                return RetornaCOO.ToString();
            }
            return "";
        }
        #endregion

        #region Retorna Numero dos Cupons Fiscais Cancelados (ok rev2)
        /// <summary>
        /// Retorna o Número de Cupons Fiscais Cancelados na MFD do ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna o Número de Cupons Fiscais Cancelados</returns>
        public string retornaNumeroCuponsFiscaisCancelados(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string NumCupons = new string('\x20', 4);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_NumeroCuponsCancelados(ref NumCupons);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return NumCupons;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {

                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_CCo = new StringBuilder(13);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("19", Str_CCo);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return Str_CCo.ToString();
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szCancelado = new String(' ', 51);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Total_Cancelado(ref szCancelado);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Total_Cancelado");
                if (IRetornoEpson == 0x00)
                    return szCancelado;
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                StringBuilder NumCupomCanc = new StringBuilder("    ");
                IRetornoSweda = clsInterfaceSweda.ECF_NumeroCuponsCancelados(NumCupomCanc);
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();

                return NumCupomCanc.ToString();
            }
            return "";
        }
        #endregion
        
        #region Retorna Numero Gran Total (ok rev2)
        /// <summary>
        /// Retorna o Gran Total de Vendas do ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna o Número de Cupons Fiscais Cancelados</returns>
        public string retornaGranTotal(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {                
                string GT = new string('\x20', 18);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_GrandeTotal(ref GT);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return GT;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_CCo = new StringBuilder(13);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("19", Str_CCo);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return Str_CCo.ToString();
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szCancelado = new String(' ', 51);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Total_Cancelado(ref szCancelado);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Total_Cancelado");
                if (IRetornoEpson == 0x00)
                    return szCancelado;
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                StringBuilder NumCupomCanc = new StringBuilder("    ");
                IRetornoSweda = clsInterfaceSweda.ECF_NumeroCuponsCancelados(NumCupomCanc);
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();

                return NumCupomCanc.ToString();
            }
            return "";
        }
        #endregion
        
        #region Retorna Numero de Intervencoes Técnicas Efetuadas no ECF (ok rev2)
        /// <summary>
        /// Retorna o Numero de Intervencoes Técnicas Feitas no Aparelho (Nota: Epson e Daruma não contém esse método em suas dlls)
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna o numero de intervenções efetuadas</returns>
        public string retornaNumeroIntervencoesTecnicas(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string NumIntervencoes = new string('\x20', 4);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_NumeroIntervencoes(ref NumIntervencoes);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return NumIntervencoes;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                return "Desculpe não foi localizado nenhum método para obter as intervenções efetuadas em seu ECF...";
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                return "Desculpe não foi localizado nenhum método para obter as intervenções efetuadas em seu ECF...";
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                StringBuilder Interven = new StringBuilder("    ");
                IRetornoSweda = clsInterfaceSweda.ECF_NumeroIntervencoes(Interven);
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                clsInterfaceSweda.Analisa_Retorno_ECF();
                return Interven.ToString();

            }
            return "";
        }
        #endregion

        #region Retorna Numero de Série da Impressora (ok rev2)
        /// <summary>
        /// Retorna Número de Série do ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna o Numero de Série</returns>
        public string retornaNumeroSerieImpressora(string modeloImpressora)
        {           
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string NumSerie = new string('\x20', 15);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_NumeroSerie(ref NumSerie);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return (NumSerie);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_Serie = new StringBuilder(20);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("77", Str_Serie);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return Str_Serie.ToString();
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                StringBuilder NumeroSerie = new StringBuilder("               ");
                IRetornoSweda = clsInterfaceSweda.ECF_NumeroSerie(NumeroSerie);
                clsInterfaceSweda.Analisa_Retorno_ECF();

                return NumeroSerie.ToString();
            }
            if (modeloImpressora.ToUpper() == "EPSON")
            {
                AbrePortaSerialSwedaEpson(modeloImpressora);
                String szDadosImpressora = "";
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Dados_Impressora(ref szDadosImpressora);

                FechaPortaSerialSwedaEpson(modeloImpressora);
                return szDadosImpressora;
            }
            return "";
        }
        #endregion

        #region Retorna Versão do FirmWare do ECF (ok rev2)
        /// <summary>
        /// Retorna Versão do FirmWare do ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna a Versão</returns>
        public string retornaVersaoFirmwareECF(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string VersaoFirmware = new string('\x20', 4);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_VersaoFirmware(ref VersaoFirmware);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return VersaoFirmware;
            }
            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                

                StringBuilder Str_Firmware = new StringBuilder(10);
                StringBuilder Str_FirmwareMF = new StringBuilder(10);
                StringBuilder Str_Data = new StringBuilder(15);
                StringBuilder Str_Seq = new StringBuilder(5);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("83", Str_Firmware);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("84", Str_FirmwareMF);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("85", Str_Data);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("86", Str_Seq);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return "Software Básico: " + Str_Firmware.ToString() + ". S.B Memória Fiscal: " + Str_FirmwareMF.ToString() + ". Data: " + Str_Data.ToString() + ". Seq: " + Str_Seq.ToString();
            }
            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                StringBuilder Firmware = new StringBuilder("    ");
                IRetornoSweda = clsInterfaceSweda.ECF_VersaoFirmware(Firmware);
                clsInterfaceSweda.Analisa_Retorno_ECF();

                return Firmware.ToString();
            }
            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szVersion = new String(' ', 8);
                string szDate = new String(' ', 8);
                string szTime = new String(' ', 6);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Versao_SWBasicoEX(ref szVersion, ref szDate, ref szTime);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Versao_SWBasicoEX");
                if (IRetornoEpson == 0x00)
                    return "Versão do Software Básico: " + szVersion + "\nData de gravação: " + szDate + "\nHora de gravação: " + szTime;
            }
            return "";
        }
        #endregion

        #region Método para Gravação de Aliquotas na Impressora Fiscal (ok rev2)
        /// <summary>
        /// Grava as Aliquotas nos ECFs
        /// </summary>
        /// <param name="aliquota">Enviar a Aliquota tipo 18,00 - 17,00, 25,00, FF, II, NN</param>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        public void gravarAliquotas(string aliquota, string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_ProgramaAliquota(aliquota, 0);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.confCadastrar_ECF_Daruma("ALIQUOTA", aliquota, ",");
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_ProgramaAliquota(aliquota, 0);
                clsInterfaceSweda.Analisa_Retorno_ECF();
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                AbrePortaSerialSwedaEpson(modeloImpressora);
                IRetornoEpson = clsInterfaceEpson.EPSON_Config_Aliquota(aliquota.Replace(",", ""), false);                
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Config_Aliquota");
                FechaPortaSerialSwedaEpson(modeloImpressora);
            }
        }
        #endregion

        #region Obtem as Aliquotas que já estão gravadas na ECF (NOTA IMPORTANTE: NÃO ACHEI MÉTODO REFERENTE PRA SWEDA, VER COM SUPORTE DELES) (pendente rev2)
        /// <summary>
        /// Obtem todas as Aliquotas gravadas na memória do ECF Fiscal
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna uma Lista das Alíquotas Gravadas</returns>
        public string ObtemAliquotasECF(string modeloImpressora)
        { 
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string rAliquotas = new string('\x20', 79);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_RetornoAliquotas(ref rAliquotas);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return rAliquotas;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                //iRetorno = DARUMA32.Daruma_FI_RetornoAliquotas(ref rAliquotas);
                StringBuilder Str_Aliquotas = new StringBuilder(300);
                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rLerAliquotas_ECF_Daruma(Str_Aliquotas);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return Str_Aliquotas.ToString();
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                //string rAliquotas = new string('\x20', 79);
                //IRetornoSweda = clsInterfaceSweda.aliquota(ref rAliquotas);
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string rAliquotas = new string('\x20', 79);
                AbrePortaSerialSwedaEpson(modeloImpressora);
                clsInterfaceEpson.EPSON_Obter_Tabela_Aliquotas(ref rAliquotas);
                FechaPortaSerialSwedaEpson(modeloImpressora);
            }
            return "";
        }
        #endregion Obtem as Aliquotas que já estão gravadas na ECF

        #region Obtem as Formas de Pagamento Já Cadastradas na ECF (NOTA IMPORTANTE: TAMBÉM NÃO ACHEI MÉTODO DA SWEDA PRA OBTER AS FORMAS DE PAGAMENTO) (pendente rev2)
        /// <summary>
        /// Obtem todas as Formas de Pagamento Efetuadas no ECF
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <returns>Retorna as Formas de Pagamento</returns>
        public string ObtemFormasDePagamento(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                string Formas = new string('\x20', 3016);
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_VerificaFormasPagamento(ref Formas);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return Formas;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                StringBuilder Str_FormaPagamento = new StringBuilder(300);

                clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("126", Str_FormaPagamento);
                //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                return Str_FormaPagamento.ToString();
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
             
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                string szPagamentos = new String(' ', 880);
                IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Tabela_Pagamentos(ref szPagamentos);
                atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Tabela_Pagamentos");
                if (IRetornoEpson == 0x00)
                    return "Tabela de Pagamentos: " + szPagamentos;
            }

            return "";
        }
        #endregion Obtem as Aliquotas que já estão gravadas na ECF

        #region Altera a impressora do horario de verao (ok Rev2)
        /// <summary>
        /// Altera a Impressora para Horário de Verão
        /// </summary>
        /// <param name="modeloImpressora">Modelo da Impressora (BEMATECH, DARUMA, EPSON OU SWEDA)</param>
        /// <param name="darumaAtivaOuDesativaHV">No caso da Daruma, tem um método para Ativar e outro para Desativar - enviar "ATIVA" ou "DESATIVA"</param>
        /// <returns>Retorna True ou False de Acordo com Resultado</returns>
        public bool alteracaoHorarioVerao(string modeloImpressora, string darumaAtivaOuDesativaHV)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_ProgramaHorarioVerao();
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                return true;
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                if (darumaAtivaOuDesativaHV == "ATIVA")
                {
                    //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                    //if (IRetornoDaruma != 1)
                    //{
                    //    MessageBox.Show("Impressora desligada");
                    //}
                    //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                    clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.confHabilitarHorarioVerao_ECF_Daruma();
                    //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                }
                else
                {
                    //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                    //if (IRetornoDaruma != 1)
                    //{
                    //    MessageBox.Show("Impressora desligada");
                    //}
                    //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                    clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.confDesabilitarHorarioVerao_ECF_Daruma();
                    //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                }
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_ProgramaHorarioVerao();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                IRetornoEpson = clsInterfaceSweda.ECF_ProgramaHorarioVerao();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoEpson);
            }
            return true;
        }
        #endregion

        #endregion Métodos referentes a Gerenciamento, Configuração, Relatórios e Totalizadores dos ECFs (ok Rev2)

        #region Método para Impressão de um Cupom Fiscal
        /// <summary>
        /// Método que Imprime o Cupom Fiscal para os ECFs programados (novo método 072013)
        /// </summary>
        /// <param name="cpfCnpj">CPF ou CNPJ caso queira Nota Fiscal Paulista</param>
        /// <param name="nomeConsumidor">Nome do Consumidor</param>
        /// <param name="modeloDaECF">Modelo do ECF (até agora Bematech, Daruma, Epson, Sweda)</param>
        /// <param name="formaPagto">String da Forma de Pagto</param>
        /// <param name="valorFinalCF">String do Valor Final do Cupom Fiscal</param>
        /// <param name="valorTotalImpostosPagos">String Valor Total dos Impostos Pagos</param>
        /// <param name="dt_DadosCupom">DataTable com os Itens da Impressão (doc. na tela de cupom ou nesse método)</param>
        /// <param name="codigoPreVenda">Código da Pré-Venda (orçamento/venda)</param>
        /// <param name="mensagemAgradecimento">Mensagem de Agradecimento para Sair no Total do Cupom</param>
        /// <param name="numeroUsuarioLogado">Número do Usuário Logado</param>
        /// <param name="nomeUsuarioLogado">Nome do Usuário Logado</param>
        /// <param name="nomeHost">Nome do Host</param>        
        /// <returns>Retorna o CCF (Código de Cupom Fiscal) Gerado</returns>
        public String imprimeCupomFiscal(string cpfCnpj, string nomeConsumidor, string modeloDaECF, string formaPagto, string valorFinalCF, string acrescimoTotal, string valorTotalImpostosPagos, DataTable dt_DadosCupom, string codigoPreVenda, string mensagemAgradecimento, int numeroUsuarioLogado, string nomeUsuarioLogado, string nomeHost)
        {
            Int32 cupomGerado = 0;
            clsNewContasMatematicas contas = new clsNewContasMatematicas();
            modeloDaECF =  modeloDaECF.ToUpper();

            #region Bematech
            if (modeloDaECF.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_AbreCupom(cpfCnpj);
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                if (dt_DadosCupom.Rows.Count > 0)
                {                       
                    for (int i = 0; i < dt_DadosCupom.Rows.Count; i++)
                    {
                        string codigoFabr = dt_DadosCupom.Rows[i]["PK_IDVenda"].ToString() +"-"+ dt_DadosCupom.Rows[i]["CodigoFabric"].ToString();
                        string descricaoProd = dt_DadosCupom.Rows[i]["Descricao"].ToString();
                        string icms = dt_DadosCupom.Rows[i]["ICMS"].ToString();
                        //string aliqEspecial = dt_DadosCupom.Rows[i]["AliqEspecial"].ToString();
                        string quantidade = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["Quantidade"].ToString());
                        string valorUnit = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["ValorBrutoUnit"].ToString());
                        string desconto = contas.newValidaAjustaArredonda2CasasDecimais(dt_DadosCupom.Rows[i]["Desconto"].ToString());
                        string acrescimo = contas.newValidaAjustaArredonda2CasasDecimais(dt_DadosCupom.Rows[i]["Acrescimo"].ToString());
                        string valorTotal = dt_DadosCupom.Rows[i]["ValorTotal"].ToString();
                        string porcImpPago = dt_DadosCupom.Rows[i]["PorcImpPago"].ToString();
                        string valorImpPago = dt_DadosCupom.Rows[i]["ValorImpPago"].ToString();
                        string validacao = dt_DadosCupom.Rows[i]["Validacao"].ToString();
                        string unidadeMedida = dt_DadosCupom.Rows[i]["UnidadeMedida"].ToString();


                        if (icms == "1")
                        {
                            icms = "07,00";
                        }

                        if (icms == "2")
                        {
                            icms = "12,00";
                        }

                        if (icms == "3")
                        {
                            icms = "13,00";
                        }

                        if (icms == "4")
                        {
                            icms = "15,00";
                        }

                        if (icms == "5")
                        {
                            icms = "17,00";
                        }

                        if (icms == "6")
                        {
                            icms = "18,00";
                        }

                        if (icms == "7")
                        {
                            icms = "25,00";
                        }

                        if (icms == "8")
                        {
                            icms = "II";
                        }

                        if (icms == "9")
                        {
                            icms = "FF";
                        }

                        if (icms == "10")
                        {
                            icms = "NN";
                        }


                        //VALIDA A QUANTIDADE DE CARACTERS PERMITIDOS NAS STRINGS PRO ECF NÃO RECUSAR
                        if (codigoFabr.Length > 13)
                        {
                            codigoFabr = codigoFabr.Substring(0, 13);
                        }

                        if(descricaoProd.Length > 29)
                        {
                            descricaoProd = descricaoProd.Substring(0,29);
                        }

                        //Holly Shit - Método 
                        //IRetornoBematech = clsInterfaceBematech.Bematech_FI_VendeItem(codigoFabr, descricaoProd, icms, "F", quantidade, 3, valorUnit, "$", desconto);
                        IRetornoBematech = clsInterfaceBematech.Bematech_FI_VendeItemDepartamento(codigoFabr, descricaoProd, icms, valorUnit, quantidade, acrescimo, desconto, "01", unidadeMedida);//"F", quantidade, 3, valorUnit, "$", desconto);                        

                        codigoFabr = null;
                        descricaoProd = null;
                        icms = null;
                        
                        quantidade = null;
                        valorUnit = null;
                        desconto = null;
                        acrescimo = null;
                        valorTotal = null;
                        porcImpPago = null;
                        valorImpPago = null;
                        validacao = null;
                    }//final do FOR

                    if (acrescimoTotal != "" && acrescimoTotal != "0,00")
                    {
                        IRetornoBematech = clsInterfaceBematech.Bematech_FI_IniciaFechamentoCupom("A", "$", contas.newValidaAjustaArredonda2CasasDecimais(acrescimoTotal));
                        clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    }
                    else
                    {
                        IRetornoBematech = clsInterfaceBematech.Bematech_FI_IniciaFechamentoCupom("D", "$", "0");
                        clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    }

                    if (formaPagto != "")
                    {
                        string primeiroCaracter = formaPagto.Substring(0, 1);
                        if(contas.verificaSeEInteiro(primeiroCaracter.ToString()))
                        {
                            formaPagto = formaPagto.Substring(11, formaPagto.Length - 11); //remove os números do plano de contas da frente da descrição da forma de pagamento... Whas...
                        }

                        if (formaPagto.Length > 16)
                        {
                            formaPagto = formaPagto.Substring(0, 16);
                        }
                        IRetornoBematech = clsInterfaceBematech.Bematech_FI_EfetuaFormaPagamentoMFD(formaPagto, contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), "1", "");
                        clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    }
                    else
                    {
                        IRetornoBematech = clsInterfaceBematech.Bematech_FI_EfetuaFormaPagamentoMFD("DINHEIRO", contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), "1", "");
                        clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    }

                    if (codigoPreVenda != "")
                    {
                        codigoPreVenda = "Fonte: IBPT.     Codigo Pre-Venda: " + codigoPreVenda;
                        if (codigoPreVenda.Length > 47)
                        {
                            codigoPreVenda = codigoPreVenda.Substring(0, 47);
                        }

                        for (int i = codigoPreVenda.Length; i < 48; i++)
                        {
                            codigoPreVenda = codigoPreVenda + " ";
                        }
                    }

                    string atendente = "Atendente: " + nomeUsuarioLogado;

                    if (atendente.Length > 47)
                    {
                        atendente = atendente.Substring(0, 47);
                    }

                    for (int i = atendente.Length; i < 48; i++)
                    {
                        atendente = atendente + " ";
                    }

                    int contadorMensagemAgradecimento = mensagemAgradecimento.Length;

                    if (contadorMensagemAgradecimento > 96)
                    {
                        mensagemAgradecimento = mensagemAgradecimento.Substring(0, 96);
                    }

                    if (contadorMensagemAgradecimento < 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 48; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    if (contadorMensagemAgradecimento < 96 && contadorMensagemAgradecimento > 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 96; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    //string sistema = "Sis.FuturaData 114255-2428-www.futuradata.com.br";
                    string sistema = "*Sistemas: (11)4255-2428 - www.futuradata.com.br";


                    //regra de 3 pra pegar o total de porcentagem do imposto pago
                    decimal valorFinalCupomFiscal = Convert.ToDecimal(valorFinalCF);
                    decimal valorImpostoPago = Convert.ToDecimal(valorTotalImpostosPagos);

                    decimal totalPorcentagemImpostoPago = (valorImpostoPago * 100) / valorFinalCupomFiscal;
                    string mensagemImpostosNota = "";
                    if (valorTotalImpostosPagos != "0" && valorTotalImpostosPagos != "0,00" && valorTotalImpostosPagos != "0,0000")
                    {
                        mensagemImpostosNota = "Val Aprox dos Tributos = R$" + valorTotalImpostosPagos.ToString() + " (" + contas.newValidaAjustaArredonda2CasasDecimais(totalPorcentagemImpostoPago.ToString()) + "%)";

                        while (mensagemImpostosNota.Length < 48)
                        {
                            mensagemImpostosNota = mensagemImpostosNota + " ";
                        }
                    }
                    
                    
                    if (cpfCnpj == "")
                    {
                        sistema = sistema + "Cliente optou por não incluir CPF/CNPJ.";
                    }

                    if (nomeConsumidor != "")
                    {
                        nomeConsumidor = "Nome consumidor:" + nomeConsumidor;
                    }
                    //Termina Fechamento Cupom


                    IRetornoBematech = clsInterfaceBematech.Bematech_FI_TerminaFechamentoCupom(mensagemImpostosNota + codigoPreVenda + mensagemAgradecimento + sistema + nomeConsumidor);
                    clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);

                    string cco = new string('\x20', 14);
                    IRetornoBematech = clsInterfaceBematech.Bematech_FI_NumeroCupom(ref cco);
                    clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);

                    return cco;                    
                }//fim if dt_Dados.Rows.Count>0
            }
            #endregion BEMATECH;

            #region DARUMA
            if (modeloDaECF.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                IRetornoDaruma = clsInterfaceDaruma.iCFAbrir_ECF_Daruma(cpfCnpj, nomeConsumidor, "");
                //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
                if (dt_DadosCupom.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_DadosCupom.Rows.Count; i++)
                    {
                        string codigoFabr = dt_DadosCupom.Rows[i]["PK_IDVenda"].ToString() + "-" + dt_DadosCupom.Rows[i]["CodigoFabric"].ToString();
                        string descricaoProd = dt_DadosCupom.Rows[i]["Descricao"].ToString();
                        string icms = dt_DadosCupom.Rows[i]["ICMS"].ToString();
                        //string aliqEspecial = dt_DadosCupom.Rows[i]["AliqEspecial"].ToString();
                        string quantidade = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["Quantidade"].ToString());
                        string valorUnit = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["ValorBrutoUnit"].ToString());
                        string desconto = contas.newValidaAjustaArredonda2CasasDecimais(dt_DadosCupom.Rows[i]["Desconto"].ToString());
                        string acrescimo = dt_DadosCupom.Rows[i]["Acrescimo"].ToString();
                        string valorTotal = dt_DadosCupom.Rows[i]["ValorTotal"].ToString();
                        string porcImpPago = dt_DadosCupom.Rows[i]["PorcImpPago"].ToString();
                        string valorImpPago = dt_DadosCupom.Rows[i]["ValorImpPago"].ToString();
                        string validacao = dt_DadosCupom.Rows[i]["Validacao"].ToString();


                        if (icms == "1")
                        {
                            icms = "07,00";
                        }

                        if (icms == "2")
                        {
                            icms = "12,00";
                        }

                        if (icms == "3")
                        {
                            icms = "13,00";
                        }

                        if (icms == "4")
                        {
                            icms = "15,00";
                        }

                        if (icms == "5")
                        {
                            icms = "17,00";
                        }

                        if (icms == "6")
                        {
                            icms = "18,00";
                        }

                        if (icms == "7")
                        {
                            icms = "25,00";
                        }

                        if (icms == "8")
                        {
                            icms = "II";
                        }

                        if (icms == "9")
                        {
                            icms = "FF";
                        }

                        if (icms == "10")
                        {
                            icms = "NN";
                        }


                        //VALIDA A QUANTIDADE DE CARACTERS PERMITIDOS NAS STRINGS PRO ECF NÃO RECUSAR
                        if (codigoFabr.Length > 13)
                        {
                            codigoFabr = codigoFabr.Substring(0, 13);
                        }

                        if (descricaoProd.Length > 29)
                        {
                            descricaoProd = descricaoProd.Substring(0, 29);
                        }

                        string tipoDescAcre = "D%";
                        string valorDeDescontoOuAcre = "0,00";
                        if(desconto != "0,00")
                        {
                            tipoDescAcre = "D$";
                            valorDeDescontoOuAcre = desconto;
                        }
                        if(acrescimo != "0,00")
                        {
                            tipoDescAcre = "A$";
                            valorDeDescontoOuAcre = desconto;
                        }
                                                
                        IRetornoDaruma = clsInterfaceDaruma.iCFVender_ECF_Daruma(icms, quantidade, valorUnit, tipoDescAcre, valorDeDescontoOuAcre, codigoFabr, "UN", descricaoProd);
                        //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);


                        //IRetornoBematech = clsInterfaceBematech.Bematech_FI_VendeItem(codigoFabr, descricaoProd, icms, "F", quantidade, 3, valorUnit, "$", desconto);

                        codigoFabr = null;
                        descricaoProd = null;
                        icms = null;
                        //aliqEspecial = null;
                        quantidade = null;
                        valorUnit = null;
                        desconto = null;
                        acrescimo = null;
                        valorTotal = null;
                        porcImpPago = null;
                        valorImpPago = null;
                        validacao = null;
                    }//final do FOR

                    if (acrescimoTotal != "" && acrescimoTotal != "0,00")
                    {
                        IRetornoDaruma = clsInterfaceDaruma.iCFTotalizarCupom_ECF_Daruma("A$", acrescimoTotal);
                        //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
                    }
                    else
                    {
                        IRetornoDaruma = clsInterfaceDaruma.iCFTotalizarCupom_ECF_Daruma("D$", "0,00");
                        //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
                    }


                    if (codigoPreVenda != "")
                    {
                        codigoPreVenda = "Fonte:  IBPT.  Codigo Pre-Venda: " + codigoPreVenda;
                        if (codigoPreVenda.Length > 47)
                        {
                            codigoPreVenda = codigoPreVenda.Substring(0, 47);
                        }

                        for (int i = codigoPreVenda.Length; i < 48; i++)
                        {
                            codigoPreVenda = codigoPreVenda + " ";
                        }
                    }

                    string atendente = "Atendente: " + nomeUsuarioLogado;

                    if (atendente.Length > 47)
                    {
                        atendente = atendente.Substring(0, 47);
                    }

                    for (int i = atendente.Length; i < 48; i++)
                    {
                        atendente = atendente + " ";
                    }

                    int contadorMensagemAgradecimento = mensagemAgradecimento.Length;

                    if (contadorMensagemAgradecimento > 96)
                    {
                        mensagemAgradecimento = mensagemAgradecimento.Substring(0, 96);
                    }

                    if (contadorMensagemAgradecimento < 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 48; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    if (contadorMensagemAgradecimento < 96 && contadorMensagemAgradecimento > 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 96; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    //string sistema = "Sis.FuturaData 114255-2428-www.futuradata.com.br";
                    string sistema = "*Sistemas: (11)4255-2428 - www.futuradata.com.br";


                    //regra de 3 pra pegar o total de porcentagem do imposto pago
                    decimal valorFinalCupomFiscal = Convert.ToDecimal(valorFinalCF);
                    decimal valorImpostoPago = Convert.ToDecimal(valorTotalImpostosPagos);

                    decimal totalPorcentagemImpostoPago = (valorImpostoPago * 100) / valorFinalCupomFiscal;

                    string mensagemImpostosNota = "Val Aprox dos Tributos = R$" + valorTotalImpostosPagos.ToString() + " (" + contas.newValidaAjustaArredonda2CasasDecimais(totalPorcentagemImpostoPago.ToString()) + "%)";


                    while (mensagemImpostosNota.Length < 48)
                    {
                        mensagemImpostosNota = mensagemImpostosNota + " ";
                    }

                    if (cpfCnpj == "")
                    {
                        sistema = sistema + "Cliente optou por não incluir CPF/CNPJ.";
                    }

                    //if (nomeConsumidor != "")
                    //{
                    //    nomeConsumidor = "Nome consumidor:" + nomeConsumidor;
                    //}
                    //Termina Fechamento Cupom


                    if (formaPagto != "")
                    {
                        if (formaPagto.Length > 16)
                        {
                            formaPagto = formaPagto.Substring(0, 16);
                        }
                        IRetornoDaruma = clsInterfaceDaruma.iCFEfetuarPagamento_ECF_Daruma(formaPagto, contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), mensagemImpostosNota + codigoPreVenda + mensagemAgradecimento + nomeConsumidor);
                        //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
                        IRetornoDaruma = clsInterfaceDaruma.iCFEncerrarConfigMsg_ECF_Daruma(sistema);
                    }
                    else
                    {
                        IRetornoDaruma = clsInterfaceDaruma.iCFEfetuarPagamento_ECF_Daruma("DINHEIRO", contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), mensagemImpostosNota + codigoPreVenda + mensagemAgradecimento + sistema + nomeConsumidor);
                        //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
                        IRetornoDaruma = clsInterfaceDaruma.iCFEncerrarConfigMsg_ECF_Daruma(sistema);
                    }

                    StringBuilder Str_CCo = new StringBuilder();     
                    clsInterfaceDaruma.iRetorno = clsInterfaceDaruma.rRetornarInformacao_ECF_Daruma("26", Str_CCo);
                    //clsInterfaceDaruma.TrataRetorno(clsInterfaceDaruma.iRetorno);
                    IRetornoDaruma = clsInterfaceDaruma.iRetorno;
                    if(Str_CCo.Length > 6)
                    {
                        return Str_CCo.ToString().Substring(0,6);
                    }
                    return Str_CCo.ToString();                                        
                }//fim if dt_Dados.Rows.Count>0
            }
            #endregion DARUMA

            #region EPSON
            if (modeloDaECF.ToUpper() == "EPSON")
            {
                AbrePortaSerialSwedaEpson(modeloDaECF);
                IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Abrir_Cupom(cpfCnpj, nomeConsumidor, "", "", 2);

                if (dt_DadosCupom.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_DadosCupom.Rows.Count; i++)
                    {
                        string codigoFabr = dt_DadosCupom.Rows[i]["PK_IDVenda"].ToString() + "-" + dt_DadosCupom.Rows[i]["CodigoFabric"].ToString();
                        string descricaoProd = dt_DadosCupom.Rows[i]["Descricao"].ToString();
                        string icms = dt_DadosCupom.Rows[i]["ICMS"].ToString();
                        string aliqEspecial = dt_DadosCupom.Rows[i]["AliqEspecial"].ToString();
                        string quantidade = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["Quantidade"].ToString());
                        string valorUnit = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["ValorUnit"].ToString());
                        string desconto = contas.newValidaAjustaArredonda2CasasDecimais(dt_DadosCupom.Rows[i]["Desconto"].ToString());
                        string acrescimo = dt_DadosCupom.Rows[i]["Acrescimo"].ToString();
                        string valorTotal = dt_DadosCupom.Rows[i]["ValorTotal"].ToString();
                        string porcImpPago = dt_DadosCupom.Rows[i]["PorcImpPago"].ToString();
                        string valorImpPago = dt_DadosCupom.Rows[i]["ValorImpPago"].ToString();
                        string validacao = dt_DadosCupom.Rows[i]["Validacao"].ToString();


                        if (icms == "1")
                        {
                            icms = "07,00";
                        }

                        if (icms == "2")
                        {
                            icms = "12,00";
                        }

                        if (icms == "3")
                        {
                            icms = "13,00";
                        }

                        if (icms == "4")
                        {
                            icms = "15,00";
                        }

                        if (icms == "5")
                        {
                            icms = "17,00";
                        }

                        if (icms == "6")
                        {
                            icms = "18,00";
                        }

                        if (icms == "7")
                        {
                            icms = "25,00";
                        }

                        if (icms == "8")
                        {
                            icms = "II";
                        }

                        if (icms == "9")
                        {
                            icms = "FF";
                        }

                        if (icms == "10")
                        {
                            icms = "NN";
                        }


                        //VALIDA A QUANTIDADE DE CARACTERS PERMITIDOS NAS STRINGS PRO ECF NÃO RECUSAR
                        if (codigoFabr.Length > 13)
                        {
                            codigoFabr = codigoFabr.Substring(0, 13);
                        }

                        if (descricaoProd.Length > 29)
                        {
                            descricaoProd = descricaoProd.Substring(0, 29);
                        }
                                                
                        IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Vender_Item(codigoFabr, descricaoProd, quantidade, 3, "UN", valorUnit, 2, icms.Replace(",", ""), 1);
                        atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Vender_Item");

                        if (acrescimo != "0,00" && acrescimo != "0,0000" && acrescimo != "")
                        {
                            IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Desconto_Acrescimo_Item(contas.newValidaAjustaArredonda2CasasDecimais(acrescimo), 2, false, false);
                            atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Desconto_Acrescimo_Item");
                        }
                        
                        codigoFabr = null;
                        descricaoProd = null;
                        icms = null;
                        aliqEspecial = null;
                        quantidade = null;
                        valorUnit = null;
                        desconto = null;
                        acrescimo = null;
                        valorTotal = null;
                        porcImpPago = null;
                        valorImpPago = null;
                        validacao = null;
                    }//final do FOR

                    //if (acrescimoTotal != "" && acrescimoTotal != "0,00")
                    //{
                    //    IRetornoBematech = clsInterfaceBematech.Bematech_FI_IniciaFechamentoCupom("A", "$", contas.newValidaAjustaArredonda2CasasDecimais(acrescimoTotal));
                    //    clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    //}
                    //else
                    //{
                    //IRetornoBematech = clsInterfaceBematech.Bematech_FI_IniciaFechamentoCupom("D", "$", "0");
                    //clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
                    //}

                    if (formaPagto != "")
                    {
                        if (formaPagto.Length > 16)
                        {
                            formaPagto = formaPagto.Substring(0, 16);
                        }


                        IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Pagamento(formaPagto, contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF.ToString()).Replace(",", ""), 2, "", "");
                        atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Pagamento");                                                
                    }
                    else
                    {
                        IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Pagamento("DINHEIRO", contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF.ToString()).Replace(",", ""), 2, "", "");
                        atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Pagamento");                                                
                    }

                    if (codigoPreVenda != "")
                    {
                        codigoPreVenda = "Fonte: IBPT.     Codigo Pre-Venda: " + codigoPreVenda;
                        if (codigoPreVenda.Length > 47)
                        {
                            codigoPreVenda = codigoPreVenda.Substring(0, 47);
                        }

                        for (int i = codigoPreVenda.Length; i < 48; i++)
                        {
                            codigoPreVenda = codigoPreVenda + " ";
                        }
                    }

                    string atendente = "Atendente: " + nomeUsuarioLogado;

                    if (atendente.Length > 47)
                    {
                        atendente = atendente.Substring(0, 47);
                    }

                    for (int i = atendente.Length; i < 48; i++)
                    {
                        atendente = atendente + " ";
                    }

                    int contadorMensagemAgradecimento = mensagemAgradecimento.Length;

                    if (contadorMensagemAgradecimento > 96)
                    {
                        mensagemAgradecimento = mensagemAgradecimento.Substring(0, 96);
                    }

                    if (contadorMensagemAgradecimento < 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 48; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    if (contadorMensagemAgradecimento < 96 && contadorMensagemAgradecimento > 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 96; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    //string sistema = "Sis.FuturaData 114255-2428-www.futuradata.com.br";
                    string sistema = "*Sistemas: (11)4255-2428 - www.futuradata.com.br";


                    //regra de 3 pra pegar o total de porcentagem do imposto pago
                    decimal valorFinalCupomFiscal = Convert.ToDecimal(valorFinalCF);
                    decimal valorImpostoPago = Convert.ToDecimal(valorTotalImpostosPagos);

                    decimal totalPorcentagemImpostoPago = (valorImpostoPago * 100) / valorFinalCupomFiscal;

                    string mensagemImpostosNota = "Val Aprox dos Tributos = R$" + valorTotalImpostosPagos.ToString() + " (" + contas.newValidaAjustaArredonda2CasasDecimais(totalPorcentagemImpostoPago.ToString()) + "%)";


                    while (mensagemImpostosNota.Length < 48)
                    {
                        mensagemImpostosNota = mensagemImpostosNota + " ";
                    }

                    if (cpfCnpj == "")
                    {
                        sistema = sistema + "Cliente optou por não incluir CPF/CNPJ.";
                    }

                    if (nomeConsumidor != "")
                    {
                        nomeConsumidor = "Nome consumidor:" + nomeConsumidor;
                    }
                    //Termina Fechamento Cupom


                    IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Fechar_Cupom(true, false);
                    atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Fechar_Cupom");

                    //8========================> Esse código está me fodendo - Fer - 16/07/2013 10:26 - Fire.


                    IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Imprimir_Mensagem(mensagemImpostosNota + codigoPreVenda, mensagemAgradecimento, sistema, nomeConsumidor, "", "", "", "");
                    atualizaRetornoEpson(IRetornoEpson, "EPSON_Fiscal_Imprimir_Mensagem");
                    
                    string szInfo = new String(' ', 30);
                    IRetornoEpson = clsInterfaceEpson.EPSON_Obter_Informacao_Ultimo_Documento(ref szInfo);
                    atualizaRetornoEpson(IRetornoEpson, "EPSON_Obter_Informacao_Ultimo_Documento");
                    if (IRetornoEpson == 0x00)
                        return szInfo;
                }//fim if dt_Dados.Rows.Count>0
            }
            #endregion EPSON

            #region SWEDA
            if (modeloDaECF.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_AbreCupom(cpfCnpj);
                //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                                
                if (dt_DadosCupom.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_DadosCupom.Rows.Count; i++)
                    {
                        string codigoFabr = dt_DadosCupom.Rows[i]["PK_IDVenda"].ToString() + "-" + dt_DadosCupom.Rows[i]["CodigoFabric"].ToString();
                        string descricaoProd = dt_DadosCupom.Rows[i]["Descricao"].ToString();
                        string icms = dt_DadosCupom.Rows[i]["ICMS"].ToString();
                        //string aliqEspecial = dt_DadosCupom.Rows[i]["AliqEspecial"].ToString();
                        string quantidade = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["Quantidade"].ToString());
                        string valorUnit = contas.newValidaAjustaArredonda3CasasDecimais(dt_DadosCupom.Rows[i]["ValorBrutoUnit"].ToString());
                        string desconto = contas.newValidaAjustaArredonda2CasasDecimais(dt_DadosCupom.Rows[i]["Desconto"].ToString());
                        string acrescimo = dt_DadosCupom.Rows[i]["Acrescimo"].ToString();
                        string valorTotal = dt_DadosCupom.Rows[i]["ValorTotal"].ToString();
                        string porcImpPago = dt_DadosCupom.Rows[i]["PorcImpPago"].ToString();
                        string valorImpPago = dt_DadosCupom.Rows[i]["ValorImpPago"].ToString();
                        string validacao = dt_DadosCupom.Rows[i]["Validacao"].ToString();


                        if (icms == "1")
                        {
                            icms = "07,00";
                        }

                        if (icms == "2")
                        {
                            icms = "12,00";
                        }

                        if (icms == "3")
                        {
                            icms = "13,00";
                        }

                        if (icms == "4")
                        {
                            icms = "15,00";
                        }

                        if (icms == "5")
                        {
                            icms = "17,00";
                        }

                        if (icms == "6")
                        {
                            icms = "18,00";
                        }

                        if (icms == "7")
                        {
                            icms = "25,00";
                        }

                        if (icms == "8")
                        {
                            icms = "II";
                        }

                        if (icms == "9")
                        {
                            icms = "FF";
                        }

                        if (icms == "10")
                        {
                            icms = "NN";
                        }


                        //VALIDA A QUANTIDADE DE CARACTERS PERMITIDOS NAS STRINGS PRO ECF NÃO RECUSAR
                        if (codigoFabr.Length > 13)
                        {
                            codigoFabr = codigoFabr.Substring(0, 13);
                        }

                        if (descricaoProd.Length > 29)
                        {
                            descricaoProd = descricaoProd.Substring(0, 29);
                        }


                        IRetornoSweda = clsInterfaceSweda.ECF_VendeItem(codigoFabr, descricaoProd, icms, "F", quantidade, 3, valorUnit, "$", desconto);
                        //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);

                        codigoFabr = null;
                        descricaoProd = null;
                        icms = null;
                        quantidade = null;
                        valorUnit = null;
                        desconto = null;
                        acrescimo = null;
                        valorTotal = null;
                        porcImpPago = null;
                        valorImpPago = null;
                        validacao = null;
                    }//final do FOR

                    if (acrescimoTotal != "" && acrescimoTotal != "0,00")
                    {
                        IRetornoSweda = clsInterfaceSweda.ECF_IniciaFechamentoCupom("A", "$", contas.newValidaAjustaArredonda2CasasDecimais(acrescimoTotal));
                        //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    }
                    else
                    {
                        IRetornoSweda = clsInterfaceSweda.ECF_IniciaFechamentoCupom("D", "$", "0");
                        //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    }

                    if (formaPagto != "")
                    {
                        string primeiroCaracter = formaPagto.Substring(0, 1);
                        if (contas.verificaSeEInteiro(primeiroCaracter.ToString()))
                        {
                            formaPagto = formaPagto.Substring(11, formaPagto.Length - 11); //remove os números do plano de contas da frente da descrição da forma de pagamento... Whas...
                        }

                        if (formaPagto.Length > 16)
                        {
                            formaPagto = formaPagto.Substring(0, 16);
                        }
                        IRetornoSweda = clsInterfaceSweda.ECF_EfetuaFormaPagamentoMFD(formaPagto, contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), "1", "");
                        //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    }
                    else
                    {
                        IRetornoSweda = clsInterfaceSweda.ECF_EfetuaFormaPagamentoMFD("DINHEIRO", contas.newValidaAjustaArredonda2CasasDecimais(valorFinalCF), "1", "");
                        //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    }

                    if (codigoPreVenda != "")
                    {
                        codigoPreVenda = "Fonte: IBPT.     Codigo Pre-Venda: " + codigoPreVenda;
                        if (codigoPreVenda.Length > 47)
                        {
                            codigoPreVenda = codigoPreVenda.Substring(0, 47);
                        }

                        for (int i = codigoPreVenda.Length; i < 48; i++)
                        {
                            codigoPreVenda = codigoPreVenda + " ";
                        }
                    }

                    string atendente = "Atendente: " + nomeUsuarioLogado;

                    if (atendente.Length > 47)
                    {
                        atendente = atendente.Substring(0, 47);
                    }

                    for (int i = atendente.Length; i < 48; i++)
                    {
                        atendente = atendente + " ";
                    }

                    int contadorMensagemAgradecimento = mensagemAgradecimento.Length;

                    if (contadorMensagemAgradecimento > 96)
                    {
                        mensagemAgradecimento = mensagemAgradecimento.Substring(0, 96);
                    }

                    if (contadorMensagemAgradecimento < 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 48; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    if (contadorMensagemAgradecimento < 96 && contadorMensagemAgradecimento > 48)
                    {
                        for (int i = mensagemAgradecimento.Length; i < 96; i++)
                        {
                            mensagemAgradecimento = mensagemAgradecimento + " ";
                        }
                    }

                    //string sistema = "Sis.FuturaData 114255-2428-www.futuradata.com.br";
                    string sistema = "*Sistemas: (11)4255-2428 - www.futuradata.com.br";


                    //regra de 3 pra pegar o total de porcentagem do imposto pago
                    decimal valorFinalCupomFiscal = Convert.ToDecimal(valorFinalCF);
                    decimal valorImpostoPago = Convert.ToDecimal(valorTotalImpostosPagos);

                    decimal totalPorcentagemImpostoPago = (valorImpostoPago * 100) / valorFinalCupomFiscal;
                    string mensagemImpostosNota = "";
                    if (valorTotalImpostosPagos != "0" && valorTotalImpostosPagos != "0,00" && valorTotalImpostosPagos != "0,0000")
                    {
                        mensagemImpostosNota = "Val Aprox dos Tributos = R$" + valorTotalImpostosPagos.ToString() + " (" + contas.newValidaAjustaArredonda2CasasDecimais(totalPorcentagemImpostoPago.ToString()) + "%)";

                        while (mensagemImpostosNota.Length < 48)
                        {
                            mensagemImpostosNota = mensagemImpostosNota + " ";
                        }
                    }


                    if (cpfCnpj == "")
                    {
                        sistema = sistema + "Cliente optou por não incluir CPF/CNPJ.";
                    }

                    if (nomeConsumidor != "")
                    {
                        nomeConsumidor = "Nome consumidor:" + nomeConsumidor;
                    }
                    //Termina Fechamento Cupom


                    IRetornoSweda = clsInterfaceSweda.ECF_TerminaFechamentoCupom(mensagemImpostosNota + codigoPreVenda + mensagemAgradecimento + sistema + nomeConsumidor);
                    //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    
                    StringBuilder RetornaCOO = new StringBuilder("      ");
                    IRetornoSweda = clsInterfaceSweda.ECF_RetornaCOO(RetornaCOO);
                    //clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                    return RetornaCOO.ToString();
                }//fim if dt_Dados.Rows.Count>0
            }
            #endregion SWEDA

            return "ERRO";
        }
        #endregion

        #region Método para Leitura X com Abertura de Jornada no caso da Epson (ok Rev2)
        /// <summary>
        /// Efetua a Leitura X na impressora
        /// </summary>
        /// <param name="modeloImpressora">Modelo Imp. (BEMATECH, SWEDA, DARUMA, EPSON)</param>
        public void leituraXAberturaJornada(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_LeituraX();
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);

            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                IRetornoDaruma = clsInterfaceDaruma.iLeituraX_ECF_Daruma();
                //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
            }
            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                IRetornoSweda = clsInterfaceSweda.ECF_LeituraX();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                IRetornoEpson = clsInterfaceEpson.EPSON_RelatorioFiscal_Abrir_Jornada();
                atualizaRetornoEpson(IRetornoEpson, "EPSON_RelatorioFiscal_Abrir_Jornada");
            }
        }
        #endregion

        #region Verifica se a jornada do dia está aberta (usada primariamente para impressora EPSON)
        /// <summary>
        /// Verifica se a jornada do dia está aberta, usar esse método para inicializar o dia, caso contrário a venda não poderá ser feita
        /// </summary>
        /// <param name="modeloImpressora"></param>
        /// <returns>Retorna true caso a jornada do dia já esteja aberta</returns>
        public bool JornadaAberta(string modeloImpressora)
        {
            bool Retorno = false;
            modeloImpressora = modeloImpressora.ToUpper();
            if (modeloImpressora.ToUpper() == "EPSON")
            {

                string szDados = new String(' ', 67);

                if (AbrePortaSerialSwedaEpson(modeloImpressora) == false)
                {
                    return false;
                }

                if (clsInterfaceEpson.EPSON_Obter_Dados_Jornada(ref szDados) == 0)
                {
                    if (szDados.Trim().Length > 0)
                    {
                        Retorno = true;
                    }
                }
                 

            }
            else
            {
                //pra não prejudicar os outros modelos de impressora q nao tem essa função
                Retorno = true;
            }

            return Retorno;
        }
        #endregion;
        
        #region Verifica se a jornada está aberta, se não estiver, abre a jornada do dia. Utilizada primariamente para EPSON -- Anderson 26/10/2011 - Fernando rev.2
        /// <summary>
        /// Verifica se a jornada está aberta, se não estiver, abre a jornada do dia
        /// </summary>
        /// <param name="modeloImpressora">Passar a string do modelo da impressora</param>
        public void AbreJornadaDoDia(string modeloImpressora)
        {
            modeloImpressora = modeloImpressora.ToUpper();

            if (JornadaAberta(modeloImpressora) == true)
            {
                return;
            }

            if (modeloImpressora == "EPSON")
            {
                AbrePortaSerialSwedaEpson(modeloImpressora);
                int Retorno = clsInterfaceEpson.EPSON_RelatorioFiscal_Abrir_Jornada();
                FechaPortaSerialSwedaEpson(modeloImpressora);
            }          
         }
        #endregion AbreJornada;

        #region Método para Redução Z na Impressora Fiscal (ok Rev2)
        public void reducaoZ(string modeloImpressora)
        {            
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_ReducaoZ("", "");
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {
                //IRetornoDaruma = clsInterfaceDaruma.eBuscarPortaVelocidade_ECF_Daruma();
                //if (IRetornoDaruma != 1)
                //{
                //    MessageBox.Show("Impressora desligada");
                //}
                //clsInterfaceDaruma.eDefinirProduto_Daruma("ECF");
                
                string dataIni = DateTime.Now.ToString("dd/MM/yyyy");
                string dataFim = DateTime.Now.ToString("dd/MM/yyyy");
                IRetornoDaruma = clsInterfaceDaruma.iReducaoZ_ECF_Daruma("", "");
                //clsInterfaceDaruma.TrataRetorno(IRetornoDaruma);
            }

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
                IRetornoSweda = clsInterfaceSweda.ECF_ReducaoZ("","");
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
            }

            if (modeloImpressora.ToUpper() == "EPSON")
            {
                AbrePortaSerialSwedaEpson("EPSON");
                string szCRZ = new String(' ', 4);
                IRetornoEpson = clsInterfaceEpson.EPSON_RelatorioFiscal_RZ("", "", 2, ref szCRZ);
                //atualizaRetorno(iRetorno, "EPSON_RelatorioFiscal_RZ");
                if (IRetornoEpson == 0x00)
                    System.Windows.Forms.MessageBox.Show("Redução Z: " + szCRZ, "Retorno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }
        #endregion

        #region Cancela o Ultimo Cupom Fiscal (ok Rev2)
        public void cancelaUltimoCupom(string modeloImpressora)
        {
            if (modeloImpressora.ToUpper() == "BEMATECH")
            {
                IRetornoBematech = clsInterfaceBematech.Bematech_FI_CancelaCupom();
                clsInterfaceBematech.Analisa_iRetorno(IRetornoBematech);
            }

            if (modeloImpressora.ToUpper() == "DARUMA")
            {                
                IRetornoDaruma = clsInterfaceDaruma.iCFCancelar_ECF_Daruma();
                //IRetorno = DARUMA32.Daruma_FI_CancelaCupom();                
            }
            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                
                IRetornoSweda = clsInterfaceSweda.ECF_CancelaCupom();
                clsInterfaceSweda.Analisa_Retorno_Dll(IRetornoSweda);
            }
            if (modeloImpressora.ToUpper() == "EPSON")
            {
                clsInterfaceEpson.EPSON_Serial_Abrir_Porta(38400, 0);
                IRetornoEpson = clsInterfaceEpson.EPSON_Fiscal_Cancelar_Cupom();
                clsInterfaceEpson.EPSON_Serial_Fechar_Porta();
            }
        }        
        #endregion
                
        #region Abre Porta da impressora (ok Rev2)
        public bool AbrePortaSerialSwedaEpson(string modeloImpressora)
        {
            return true;
        }
        #endregion Abre Porta da impressora;

        #region Fecha porta da Impressora (ok Rev2)
        public void FechaPortaSerialSwedaEpson(string modeloImpressora)
        {
            modeloImpressora = modeloImpressora.ToUpper();

            if (modeloImpressora.ToUpper() == "SWEDA")
            {
                clsInterfaceSweda.ECF_FechaPortaSerial();
                Thread.Sleep(0200);// para 2 segundos pra impressora respirar
            }
            if (modeloImpressora.ToUpper() == "EPSON")
            {
                IRetornoEpson = clsInterfaceEpson.EPSON_Serial_Fechar_Porta();
                Thread.Sleep(0200);// para 2 segundos pra impressora respirar
            }
        }
        #endregion Fecha porta da Impressora;

        #region Método Atualiza Retorno Epson
        private void atualizaRetornoEpson(int iRetorno, string funcName)
        {
            string szEstadoImpressora = new String(' ', 16);
            string szEstadoFiscal = new String(' ', 16);
            string szRetornoComando = new String(' ', 4);
            string szMsgErro = new String(' ', 100);

            iRetorno = clsInterfaceEpson.EPSON_Obter_Estado_ImpressoraEX(ref szEstadoImpressora, ref szEstadoFiscal, ref szRetornoComando, ref szMsgErro);

            string mensagemBoxParaExibirFuturaData = "";

            mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + funcName;
            mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + szMsgErro;

            if (iRetorno != 0)
                mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "ERRO";
            else
            {
                if (iRetorno != 0)
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "ERRO";
                else
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "SUCESSO";

                //---------------------------------------------------------------------------------------------------
                // Exibe o estado do mecanismo impressor
                //---------------------------------------------------------------------------------------------------
                //T2.Text = "";

                if (szEstadoImpressora.Substring(0, 1) == "1")	//Posição 1
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Impressora Offline - ";
                else
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Impressora Online - ";

                if (szEstadoImpressora.Substring(1, 1) == "1")	//Posição 2
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Erro de impressão - ";

                if (szEstadoImpressora.Substring(2, 1) == "1")	//Posição 3
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Tampa superior aberta - ";

                if (szEstadoImpressora.Substring(3, 1) == "1")	//Posição 4
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Gaveta = 1 - ";
                else
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Gaveta = 0 - ";

                //Posição 5 Reservada - Não utilizada

                if (szEstadoImpressora.Substring(5, 2) == "00")		//Posição 6 e 7
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Estação recibo - ";
                else if (szEstadoImpressora.Substring(5, 2) == "01")	//Posição 6 e 7
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Estação cheque - ";
                else if (szEstadoImpressora.Substring(5, 2) == "10")	//Posição 6 e 7
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Estação Autenticação - ";
                else if (szEstadoImpressora.Substring(5, 2) == "11")	//Posição 6 e 7
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Leitura do MICR - ";

                if (szEstadoImpressora.Substring(7, 1) == "1")	//Posição 8
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Aguardando retirada do papel - ";

                if (szEstadoImpressora.Substring(8, 1) == "1")	//Posição 9
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Aguardando inserção do papel - ";

                if (szEstadoImpressora.Substring(9, 1) == "1")	//Posição 10
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Sensor inferior da estação de cheque Acionado - ";

                if (szEstadoImpressora.Substring(10, 1) == "1")	//Posição 11
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Sensor superior da estação do cheque Acionado - ";

                if (szEstadoImpressora.Substring(11, 1) == "1")	//Posição 12
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Sensor de autenticação Acionado - ";

                //Posição 13 e 14 Reservada - Não utilizada

                if (szEstadoImpressora.Substring(14, 1) == "1")	//Posição 15
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Sem papel - ";

                if (szEstadoImpressora.Substring(15, 1) == "1")	//Posição 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Pouco papel - ";
                //---------------------------------------------------------------------------------------------------

                //---------------------------------------------------------------------------------------------------
                // Exibe o estado fiscal
                //---------------------------------------------------------------------------------------------------
                

                if (szEstadoFiscal.Substring(0, 2) == "00")		//Posição 1 e 2
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Modo bloqueado - ";
                else if (szEstadoFiscal.Substring(0, 2) == "10")	//Posição 1 e 2
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Modo manufatura (Não-Fiscalizado) - ";
                else if (szEstadoFiscal.Substring(0, 2) == "11")	//Posição 1 e 2
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Modo Fiscalizado - ";

                //Posição 3 Reservada - Não utilizada

                if (szEstadoFiscal.Substring(3, 1) == "1")		//Posição 4
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Modo de Intervenção Técnica - ";
                else
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Modo de operação normal - ";

                if (szEstadoFiscal.Substring(4, 2) == "00")		//Posição 5 e 6
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Memória Fiscal em operação normal - ";
                else if (szEstadoFiscal.Substring(4, 2) == "01")	//Posição 5 e 6
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Memória Fiscal em esgotamento - ";
                else if (szEstadoFiscal.Substring(4, 2) == "10")	//Posição 5 e 6
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Memória Fiscal cheia - ";
                if (szEstadoFiscal.Substring(4, 2) == "11")		//Posição 5 e 6
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Erro de leitura/escrita da Memória Fiscal - ";

                //Posições 7 e 8 Reservads - Não utilizadas

                if (szEstadoFiscal.Substring(8, 1) == "1")		//Posição 9
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Período de vendas aberto - ";
                else
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Período de vendas fechado - ";

                //Posições 10, 11 e 12 Reservads - Não utilizadas

                if (szEstadoFiscal.Substring(12, 4) == "0000")		//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Documento fechado - ";
                else if (szEstadoFiscal.Substring(12, 4) == "0001")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Cupom Fiscal aberto - ";
                else if (szEstadoFiscal.Substring(12, 4) == "0010")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Comprovante de Crédito ou Débito - ";
                else if (szEstadoFiscal.Substring(12, 4) == "0011")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Estorno de Comprovante de Crédito ou Débito - ";
                else if (szEstadoFiscal.Substring(12, 4) == "0100")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Relatório Gerencial - ";
                else if (szEstadoFiscal.Substring(12, 4) == "1000")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Comprovante Não-Fiscal - ";
                else if (szEstadoFiscal.Substring(12, 4) == "1001")	//Posição 13, 14, 15 e 16
                    mensagemBoxParaExibirFuturaData = mensagemBoxParaExibirFuturaData + " - " + "Cheque ou autenticação - ";
                //---------------------------------------------------------------------------------------------------
            }
            if (mensagemBoxParaExibirFuturaData != "")
            {
                MessageBox.Show(null, mensagemBoxParaExibirFuturaData, "FuturaData ClsFiscal Epson", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
    }//fim classe
}//fim namespace