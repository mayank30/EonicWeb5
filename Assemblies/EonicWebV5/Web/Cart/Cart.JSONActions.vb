﻿Option Strict Off
Option Explicit On
Imports System.Xml
Imports System.Collections
Imports System.Web.Configuration
Imports System.Data.SqlClient
Imports System.Web.HttpUtility
Imports VB = Microsoft.VisualBasic
Imports System.IO
Imports Eonic.Tools.Xml
Imports Eonic.Tools.Xml.XmlNodeState
Imports System

Partial Public Class Web

    Partial Public Class Cart

#Region "JSON Actions"

        Public Class JSONActions
            Public Event OnError(ByVal sender As Object, ByVal e As Eonic.Tools.Errors.ErrorEventArgs)
            Private Const mcModuleName As String = "Eonic.Cart.JSONActions"
            Private moLmsConfig As System.Collections.Specialized.NameValueCollection = WebConfigurationManager.GetWebApplicationSection("eonic/lms")
            Private myWeb As Eonic.Web
            Private myCart As Eonic.Web.Cart

            Public Sub New()
                Dim ctest As String = "this constructor is being hit" 'for testing
                myWeb = New Eonic.Web()
                myWeb.InitializeVariables()
                myWeb.Open()
                myCart = New Eonic.Web.Cart(myWeb)

            End Sub

            Public Function GetCart(ByRef myApi As Eonic.API, ByRef jObj As Newtonsoft.Json.Linq.JObject) As String
                Try
                    Dim cProcessInfo As String = ""

                    Dim CartXml As XmlElement = myWeb.moCart.CreateCartElement(myWeb.moPageXml)
                    myCart.GetCart(CartXml.FirstChild)
                    Dim jsonString As String = Newtonsoft.Json.JsonConvert.SerializeXmlNode(CartXml, Newtonsoft.Json.Formatting.Indented)
                    Return jsonString.replace("""@", """_")
                    'persist cart
                    myCart.close()

                Catch ex As Exception
                    RaiseEvent OnError(Me, New Eonic.Tools.Errors.ErrorEventArgs(mcModuleName, "GetCart", ex, ""))
                    Return ex.Message
                End Try
            End Function

            Public Function AddItems(ByRef myApi As Eonic.API, ByRef jObj As Newtonsoft.Json.Linq.JObject) As String
                Try
                    Dim cProcessInfo As String = ""
                    'jsonObject("artId")
                    ' myCart.AddItem()
                    'Output the new cart
                    Dim oDoc As New XmlDocument
                    Dim CartXml As XmlElement = myWeb.moCart.CreateCartElement(myWeb.moPageXml)

                    If myCart.mnCartId < 1 Then
                        myCart.CreateNewCart(CartXml)
                        If myCart.mcItemOrderType <> "" Then
                            myCart.mmcOrderType = myCart.mcItemOrderType
                        Else
                            myCart.mmcOrderType = ""
                        End If
                        myCart.mnProcessId = 1
                    End If

                    Dim item As Newtonsoft.Json.Linq.JObject

                    For Each item In jObj("Item")
                        myCart.AddItem(item("contentId"), item("qty"), Nothing)
                    Next

                    'Output the new cart
                    myCart.GetCart(CartXml.FirstChild)
                    'persist cart
                    myCart.close()

                    Dim jsonString As String = Newtonsoft.Json.JsonConvert.SerializeXmlNode(CartXml, Newtonsoft.Json.Formatting.Indented)
                    Return jsonString.Replace("""@", """_")

                Catch ex As Exception
                    RaiseEvent OnError(Me, New Eonic.Tools.Errors.ErrorEventArgs(mcModuleName, "GetCart", ex, ""))
                    Return ex.Message
                End Try

            End Function

            Public Function RemoveItems(ByRef myApi As Eonic.API, ByRef jObj As Newtonsoft.Json.Linq.JObject) As String
                Try
                    Dim cProcessInfo As String = ""
                    Dim ItemCount As Long = 1

                    Dim item As Newtonsoft.Json.Linq.JObject

                    For Each item In jObj("Item")
                        If item("contentId") Is Nothing Then
                            ItemCount = myCart.RemoveItem(item("itemId"), 0)
                        Else
                            ItemCount = myCart.RemoveItem(0, item("contentId"))
                        End If
                    Next

                    If ItemCount = 0 Then
                        myCart.QuitCart()
                        myCart.EndSession()
                    End If

                    'Output the new cart   
                    Dim CartXml As XmlElement = myWeb.moCart.CreateCartElement(myWeb.moPageXml)
                    myCart.GetCart(CartXml.FirstChild)
                    'persist cart
                    myCart.close()

                    Dim jsonString As String = Newtonsoft.Json.JsonConvert.SerializeXmlNode(CartXml, Newtonsoft.Json.Formatting.Indented)
                    Return jsonString.Replace("""@", """_")

                Catch ex As Exception
                    RaiseEvent OnError(Me, New Eonic.Tools.Errors.ErrorEventArgs(mcModuleName, "GetCart", ex, ""))
                    Return ex.Message
                End Try

            End Function

            Public Function UpdateItems(ByRef myApi As Eonic.API, ByRef jObj As Newtonsoft.Json.Linq.JObject) As String
                Try
                    Dim cProcessInfo As String = ""
                    Dim ItemCount As Long = 1

                    Dim item As Newtonsoft.Json.Linq.JObject

                    For Each item In jObj("Item")
                        If item("contentId") Is Nothing Then
                            If item("qty") = "0" Then
                                ItemCount = myCart.RemoveItem(item("itemId"), 0)
                            Else
                                ItemCount = myCart.UpdateItem(item("itemId"), 0, item("qty"))
                            End If
                        Else
                            If item("qty") = "0" Then
                                ItemCount = myCart.RemoveItem(0, item("contentId"))
                            Else
                                ItemCount = myCart.UpdateItem(0, item("contentId"), item("qty"))
                            End If
                        End If
                    Next

                    If ItemCount = 0 Then
                        myCart.QuitCart()
                        myCart.EndSession()
                    End If

                    'Output the new cart
                    Dim CartXml As XmlElement = myWeb.moCart.CreateCartElement(myWeb.moPageXml)
                    myCart.GetCart(CartXml.FirstChild)
                    'persist cart
                    myCart.close()

                    Dim jsonString As String = Newtonsoft.Json.JsonConvert.SerializeXmlNode(CartXml, Newtonsoft.Json.Formatting.Indented)
                    Return jsonString.Replace("""@", """_")

                Catch ex As Exception
                    RaiseEvent OnError(Me, New Eonic.Tools.Errors.ErrorEventArgs(mcModuleName, "GetCart", ex, ""))
                    Return ex.Message
                End Try

            End Function


        End Class

#End Region
    End Class

End Class

