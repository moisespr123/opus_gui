Imports System.IO
Imports System.Threading
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Services
Imports Google.Apis.Util.Store

Public Class GoogleDriveClass
    Shared ReadOnly Scopes As String() = {DriveService.Scope.DriveReadonly}
    Shared SoftwareName As String = String.Empty
    Public service As DriveService
    Public FolderList As List(Of String) = New List(Of String)
    Public FolderListID As List(Of String) = New List(Of String)
    Public FileList As List(Of String) = New List(Of String)
    Public FileListID As List(Of String) = New List(Of String)
    Public previousFolder As List(Of String) = New List(Of String)
    Public connected As Boolean = False
    Public currentFolder As String = ""
    Public currentFolderName As String = ""
    Public credential As UserCredential
    Public Sub New(ByVal AppName As String)
        SoftwareName = AppName
        Dim SectretsFile As String = String.Empty
        If File.Exists("client_secret.json") Then
            SectretsFile = "client_secret.json"
        ElseIf File.Exists("credentials.json") Then
            SectretsFile = "credentials.json"
        End If
        If Not SectretsFile = String.Empty Then
            Using stream = New FileStream(SectretsFile, FileMode.Open, FileAccess.Read)
                Dim credPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                credPath = Path.Combine(credPath, ".credentials/" & SoftwareName & ".json")
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
            End Using
            service = New DriveService(New BaseClientService.Initializer() With {
                .HttpClientInitializer = credential,
                .ApplicationName = SoftwareName
            })
            connected = True
        Else
            connected = False
        End If
    End Sub
    Private Async Function getToken(ByVal credentials As UserCredential) As Task(Of String)
        Return Await credentials.GetAccessTokenForRequestAsync()
    End Function
    Private Function GetFolderName(ByVal Id As String) As String
        Try
            Dim getRequest As FilesResource.GetRequest = service.Files.[Get](Id)
            Dim folderName As Google.Apis.Drive.v3.Data.File = getRequest.Execute()
            Return folderName.Name
        Catch
            Return "Error retrieving folder name"
        End Try
    End Function
    Public Sub GoBack()
        If previousFolder.Count > 0 Then
            GetData(previousFolder(previousFolder.Count - 1), True)
            previousFolder.RemoveAt(previousFolder.Count - 1)
        End If
    End Sub

    Public Sub GetData(ByVal folderId As String, ByVal Optional goingBack As Boolean = False, ByVal Optional refreshing As Boolean = False)
        FolderList.Clear()
        FolderListID.Clear()
        FileList.Clear()
        FileListID.Clear()
        Dim listRequestQString As String = "mimeType!='application/vnd.google-apps.folder' and '" & folderId & "' in parents and trashed = false"
        Dim listRequestQFolderString As String = "mimeType='application/vnd.google-apps.folder' and '" & folderId & "' in parents and trashed = false"
        Dim PageToken1 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.Q = listRequestQString
            listRequest.OrderBy = "name"
            listRequest.PageToken = PageToken1
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then

                    For Each file In files.Files
                        FileList.Add(file.Name)
                        FileListID.Add(file.Id)
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch
            End Try
        Loop While PageToken1 IsNot Nothing
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.Q = listRequestQFolderString
            listRequest.OrderBy = "name"
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()

                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then

                    For Each file In files.Files
                        FolderList.Add(file.Name)
                        FolderListID.Add(file.Id)
                    Next
                End If

                PageToken2 = files.NextPageToken
            Catch
            End Try
        Loop While PageToken2 IsNot Nothing
        If Not refreshing AndAlso Not goingBack AndAlso folderId <> "root" Then previousFolder.Add(currentFolder)
        currentFolder = folderId
        currentFolderName = GetFolderName(currentFolder)
    End Sub

    Public Sub DownloadFile(ByVal Id As String, ByVal stream As FileStream)
        Dim getRequest As FilesResource.GetRequest = service.Files.Get(Id)
        getRequest.Download(stream)
        stream.Close()
    End Sub
    Public Sub DownloadFile(ByVal Id As String, ByVal stream As MemoryStream)
        Dim getRequest As FilesResource.GetRequest = service.Files.Get(Id)
        getRequest.Download(stream)
    End Sub
End Class
