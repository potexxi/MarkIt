using MarkIt.settings;
using MarkIt.windows;
using Supabase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für FileBar.xaml
    /// </summary>
    public partial class FileBar : UserControl
    {
        private FileHistoryItem? selectedFileHistoryItem = null;
        private TreeViewItem? selectedTreeViewItem;
        public List<string> history;
        private DispatcherTimer timerBlurOut;
        private DispatcherTimer timerBlurIn;
        private DispatcherTimer timerUpdate;
        public FileBar()
        {
            InitializeComponent();
            timerBlurIn = new DispatcherTimer();
            timerBlurIn.Interval = TimeSpan.FromMilliseconds(1);
            timerBlurIn.Tick += TimerBlurIn_Tick;

            timerBlurOut = new DispatcherTimer();
            timerBlurOut.Interval = TimeSpan.FromMilliseconds(1);
            timerBlurOut.Tick += TimerBlurOut_Tick;

            timerUpdate = new DispatcherTimer();
            timerUpdate.Interval = TimeSpan.FromHours(6.7);
            timerUpdate.Tick += TimerUpdate_Tick;

            selectedFileHistoryItem = null;
            history = new List<string>();
        }

        private void TimerUpdate_Tick(object? sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            MainWindow.FileManager.InitFileHistory();
            DrawHistory(MainWindow.FileManager.FileHistory);
            UpdateFileTreeLocal();
            UpdateFileTreeCloud();
            BorderMain.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
            LabelRecent.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            LabelLocal.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            LabelCloud.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            TreeViewLocal.Resources[SystemColors.HighlightBrushKey] = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            TreeViewCloud.Resources[SystemColors.HighlightBrushKey] = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        public void DrawHistory(List<FileHistoryItem> history)
        {
            StackPanelHistory.Children.Clear();
            if (history == null || history.Count == 0)
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.Content = "No recent files...";
                ScrollViewerHistory.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                label.Margin = new Thickness(10, -5, 0, 0);
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                StackPanelHistory.Children.Add(label);
            }
            foreach (FileHistoryItem item in history)
            {
                if (item.Type != FileManager.FileType.Root)
                {
                    System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                    label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    label.MouseEnter += Label_MouseEnter;
                    label.MouseLeave += Label_MouseLeave;
                    label.PreviewMouseDoubleClick += LabelHistory_PreviewMouseDoubleClick;
                    label.PreviewMouseDown += LabelHistory_PreviewMouseDown;
                    label.Margin = new Thickness(10, -5, 0, 0);
                    label.Cursor = Cursors.Hand;
                    label.FontFamily = new FontFamily("Leelawadee UI");
                    label.FontSize = 14;
                    //string[] parts = item.Path.Split(MainWindow.currentUser.Email.Split("@")[0]);
                    //label.Content = parts[1];
                    if (item.Type == FileManager.FileType.Local)
                    {
                        label.Content = $"📄 {item.Path}";
                    }
                    else if (item.Type == FileManager.FileType.Cloud)
                    {
                        label.Content = $"☁ {item.Path}";
                    }
                    label.Tag = item;
                    StackPanelHistory.Children.Add(label);
                }
            }
        }

        private void LabelHistory_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            foreach (System.Windows.Controls.Label label1 in StackPanelHistory.Children)
            {
                label1.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            }
            System.Windows.Controls.Label label = (System.Windows.Controls.Label)sender;
            label.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            selectedFileHistoryItem = (FileHistoryItem)label.Tag;
            ButtonDel.IsEnabled = false;
        }

        private void LabelHistory_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ButtonOpen_Click(sender, e);
            Hide();
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Label label = (System.Windows.Controls.Label)sender;
            label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Label label = (System.Windows.Controls.Label)sender;
            if (selectedFileHistoryItem != (FileHistoryItem)label.Tag)
                label.Foreground = (Brush) new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            timerBlurIn.Stop();
            timerBlurOut.Stop();
            Hide();
        }

        public void SetSize(double width, double height)
        {
            this.Height = height;
            this.Width = width;
            RectBackground.Height = height;
            RectBackground.Width = width;
            BorderMain.Width = 0.3 * width;
            BorderMain.Height = 0.97 * height - 50;
            ScrollViewerMain.Height = 0.78 * (0.97 * height - 50);
            ScrollViewerMain.Width = 0.3 * width;
            if(RectBackground.IsHitTestVisible == false)
                Canvas.SetLeft(BorderMain, -BorderMain.Width - 20);
            ButtonDel.Width = BorderMain.Width * 0.9;
            ButtonOpen.Width = BorderMain.Width * 0.9;
            ButtonCreate.Width = BorderMain.Width * 0.9;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if(e.Handled != null)
                e.Handled = true;
            try
            {
                if (selectedTreeViewItem != null)
                {
                    (string path, FileManager.FileType type) = ((string, FileManager.FileType))selectedTreeViewItem.Tag;
                    if (type == FileManager.FileType.Root)
                    {
                        WindowMessageBox box1 = new WindowMessageBox("Cannot remove!", "You cannot remove this folder! It is your root folder.");
                        box1.ShowDialog();
                        return;
                    }
                    WindowMessageBox box;
                    if (Directory.Exists(path))
                        box = new WindowMessageBox("Really?", "Do you really want to delete this folder? All files will be removed in this folder.", WindowMessageBox.ButtonType.YesNo);
                    else
                        box = new WindowMessageBox("Really?", "Do you really want to delete this file?", WindowMessageBox.ButtonType.YesNo);
                    box.ShowDialog();
                    if (box.returnType == WindowMessageBox.ReturnType.Yes)
                    {
                        if (type == FileManager.FileType.Local)
                        {
                            if (Directory.Exists(path))
                            {
                                try
                                {
                                    Directory.Delete(path, true);
                                    RemoveItem(TreeViewLocal, selectedTreeViewItem);
                                }
                                catch (Exception ex)
                                {
                                    box = new WindowMessageBox("Access denied.", "This folder is read-only. MarkIt cannot delete it.");
                                    box.ShowDialog();
                                    Logger.logger.Warning($"Delete directory: {ex.Message}");
                                    return;
                                }
                            }
                            else if (File.Exists(path))
                            {
                                File.Delete(path);
                                RemoveItem(TreeViewLocal, selectedTreeViewItem);
                            }
                        }
                        else if (type == FileManager.FileType.Cloud)
                        {
                            MainWindow.FileManager.DeleteFromServer(path, MainWindow.loadingScreen);
                            RemoveItem(TreeViewCloud, selectedTreeViewItem);
                        }
                        selectedTreeViewItem = null;
                    }
                }
            }
            catch(Exception ex)
            {
                WindowMessageBox box = new WindowMessageBox("Unexpected Error!", "This action caused a unexpected error, please try again.");
                box.ShowDialog();
                Logger.logger.Error($"Delete file: {ex.Message}");
            }

        }

        // ChatGPT-Anfang
        // Prompt: <Funktion ohne itemscontrol und 2. if in der for loop> wieso geht diese funktion nicht bei eine file tree view
        private bool RemoveItem(ItemsControl treeView, TreeViewItem item)
        {
            selectedTreeViewItem = null;
            foreach(TreeViewItem item2 in treeView.Items)
            {
                if(item2 == item)
                {
                    treeView.Items.Remove(item);
                    return true;
                }
                if(item2 is TreeViewItem treeView1)
                {
                    if(RemoveItem(treeView1, item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // ChatGPT-Ende

        private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            try
            {
                string path;
                FileManager.FileType type;
                if (selectedTreeViewItem != null)
                {
                    (path, type) = ((string, FileManager.FileType))selectedTreeViewItem.Tag;
                }
                else if (selectedFileHistoryItem != null)
                {
                    path = selectedFileHistoryItem.Path;
                    type = selectedFileHistoryItem.Type;
                }
                else { return; }
                if (type == FileManager.FileType.Local)
                {
                    if (!File.Exists(path))
                    {
                        if (Directory.Exists(path))
                        {
                            WindowMessageBox box1 = new WindowMessageBox("This is a folder.", "You cannot open a folder!");
                            box1.ShowDialog();
                            return;
                        }
                        RemoveItem(TreeViewLocal, selectedTreeViewItem);
                        WindowMessageBox box = new WindowMessageBox("File does not exist.", "This file does not exist anymore.");
                        box.ShowDialog();
                        return;
                    }
                    MainWindow.CurrentWorkSheet.LoadFromString(MainWindow.FileManager.LoadFromFile(path));
                    MainWindow.FileManager.fileType = FileManager.FileType.Local;
                }
                else if(type == FileManager.FileType.Cloud)
                {
                    MainWindow.CurrentWorkSheet.LoadFromString(await MainWindow.FileManager.Download(path, MainWindow.loadingScreen));
                    MainWindow.FileManager.fileType = FileManager.FileType.Cloud;
                }
                Hide();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task UpdateFileTreeCloud()
        {
            TreeViewCloud.IsHitTestVisible = true;
            TreeViewCloud.Items.Clear();
            if(MainWindow.currentUser.Email != "guest")
            {
                TreeViewCloudRootCount = 0;
                TreeViewItem item = await GetAllCloudPath(MainWindow.FileManager.userPath);
                TreeViewCloud.Items.Add(item);
            }
            else
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = "You need to be logged in, \nto use this function.";
                item.FontSize = 14;
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                TreeViewCloud.IsHitTestVisible = false;
                TreeViewCloud.Items.Add(item);
            }
        }

        public void UpdateFileTreeLocal()
        {
            TreeViewLocal.IsHitTestVisible = true;
            string path = MainWindow.FileManager.userPath;
            TreeViewLocal.Items.Clear();
            DirectoryInfo userInfo = new DirectoryInfo(path);
            if (userInfo.GetFiles().Length <= 1 && userInfo.GetDirectories().Length < 0)
            {
                System.Windows.Controls.Label item = new System.Windows.Controls.Label();
                item.Content = "No local files.";
                item.FontSize = 14;
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                TreeViewLocal.Items.Add(item);
                TreeViewLocal.IsHitTestVisible = false;
                return;
            }
            TreeViewLocalRootCount = 0;
            TreeViewLocal.Items.Add(GetAllLocalPath(userInfo));
        }

        private int TreeViewLocalRootCount = 0;
        private TreeViewItem GetAllLocalPath(DirectoryInfo info)
        {
            TreeViewItem item = new TreeViewItem { Header = $"📁 {info.Name}" };
            item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            item.FontSize = 14;

            item.ContextMenu = new ContextMenu();
            item.ContextMenu.Opened += ContextMenu_Opened;
            item.ContextMenu.Closed += ContextMenu_Closed;
            MenuItem menuItem1 = new MenuItem { Header = "Create File", Cursor = Cursors.Hand };
            menuItem1.Click += MenuItemCreate_Click;
            MenuItem menuItem2 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
            menuItem2.Click += MenuItemDelete_Click;
            MenuItem menuItem3 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
            menuItem3.Click += MenuItemRename_Click;
            item.ContextMenu.Items.Add(menuItem1);
            item.ContextMenu.Items.Add(menuItem2);
            item.ContextMenu.Items.Add(menuItem3);

            if (TreeViewLocalRootCount == 0)
            {
                item.Tag = (info.FullName, FileManager.FileType.Root);
                TreeViewLocalRootCount = 1;
            }
            else
            {
                item.Tag = (info.FullName, FileManager.FileType.Local);
            }
            item.PreviewMouseDown += Fileitem_PreviewMouseDown;
            item.MouseEnter += TreeViewItem_MouseEnter;
            item.MouseLeave += TreeViewItem_MouseLeave;
            item.Cursor = Cursors.Hand;

            foreach (DirectoryInfo underfolder in info.GetDirectories())
            {
                TreeViewItem folderitem = GetAllLocalPath(underfolder);
                folderitem.PreviewMouseDown += Fileitem_PreviewMouseDown;
                folderitem.MouseEnter += TreeViewItem_MouseEnter;
                folderitem.MouseLeave += TreeViewItem_MouseLeave;
                folderitem.Tag = (underfolder.FullName, FileManager.FileType.Local);
                folderitem.Cursor = Cursors.Hand;

                ContextMenu contextMenu = new ContextMenu();
                contextMenu.Opened += ContextMenu_Opened;
                contextMenu.Closed += ContextMenu_Closed;

                MenuItem menuItem21 = new MenuItem { Header = "Create", Cursor = Cursors.Hand };
                menuItem21.Click += MenuItemCreate_Click;
                MenuItem menuItem22 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
                menuItem22.Click += MenuItemDelete_Click;
                MenuItem menuItem23 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
                menuItem23.Click += MenuItemRename_Click;
                contextMenu.Items.Add(menuItem21);
                contextMenu.Items.Add(menuItem22);
                contextMenu.Items.Add(menuItem23);
                folderitem.ContextMenu = contextMenu;

                item.Items.Add(folderitem);
            }
            foreach(FileInfo file in info.GetFiles())
            {
                if(file.Name != "file-history.json")
                {
                    TreeViewItem fileitem = new TreeViewItem { Header = file.Name };
                    fileitem.MouseDoubleClick += Fileitem_MouseDoubleClick;
                    fileitem.PreviewMouseDown += Fileitem_PreviewMouseDown;
                    fileitem.MouseEnter += TreeViewItem_MouseEnter;
                    fileitem.MouseLeave += TreeViewItem_MouseLeave;
                    fileitem.Cursor = Cursors.Hand;
                    fileitem.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    fileitem.FontSize = 14;
                    fileitem.Tag = (file.FullName, FileManager.FileType.Local);

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Opened += ContextMenu_Opened;
                    contextMenu.Closed += ContextMenu_Closed;

                    MenuItem menuItem21 = new MenuItem { Header = "Open", Cursor = Cursors.Hand };
                    menuItem21.Click += MenuItemOpen_Click;
                    MenuItem menuItem22 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
                    menuItem22.Click += MenuItemDelete_Click;
                    MenuItem menuItem23 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
                    menuItem23.Click += MenuItemRename_Click;
                    contextMenu.Items.Add(menuItem21);
                    contextMenu.Items.Add(menuItem22);
                    contextMenu.Items.Add(menuItem23);
                    fileitem.ContextMenu = contextMenu;

                    item.Items.Add(fileitem);
                }
            }
            return item;
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpen_Click(sender, e);
        }

        private void MenuItemRename_Click(object sender, RoutedEventArgs e)
        {
            if(selectedTreeViewItem != null)
            {
                (string path, FileManager.FileType type) = ((string, FileManager.FileType))selectedTreeViewItem.Tag;
                WindowRename window = new WindowRename(path, type);
                window.ShowDialog();
                UpdateFileTreeCloud();
                UpdateFileTreeLocal();
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            ButtonDel_Click(sender, e);
            UpdateFileTreeLocal();
            UpdateFileTreeCloud();
        }

        private void MenuItemCreate_Click(object sender, RoutedEventArgs e)
        {
            ButtonCreate_Click(sender, e);
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            //if (selectedTreeViewItem != null)
            //    selectedTreeViewItem.Background = Brushes.Transparent;
            selectedTreeViewItem = null;
            foreach(TreeViewItem item in TreeViewLocal.Items)
            {
                ClearTreeViewItemsBackground(item);
            }
            foreach (TreeViewItem item in TreeViewCloud.Items)
            {
                ClearTreeViewItemsBackground(item);
            }
        }

        private void ClearTreeViewItemsBackground(TreeViewItem item)
        {
            foreach(TreeViewItem item1 in item.Items)
            {
                ClearTreeViewItemsBackground(item1);
            }
            item.Background = Brushes.Transparent;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (selectedTreeViewItem != null)
                selectedTreeViewItem.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if(item != selectedTreeViewItem)
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private int TreeViewCloudRootCount = 0;
        public async Task<TreeViewItem> GetAllCloudPath(string userpath)
        {
            List<FileObject>? storageitems = await MainWindow.supabase.Storage.From("MarkIt").List(userpath);
            TreeViewItem big = new TreeViewItem();
            if (TreeViewCloudRootCount == 0)
            {
                big.Tag = (userpath, FileManager.FileType.Root);
                TreeViewCloudRootCount = 1;
            }
            else
            {
                big.Tag = (userpath, FileManager.FileType.Cloud);
            }
            big.FontSize = 14;
            big.Header = $"📁 {userpath.Split('/')[^1]}";
            big.PreviewMouseDown += Fileitem_PreviewMouseDown;
            big.Cursor = Cursors.Hand;
            big.MouseEnter += TreeViewItem_MouseEnter;
            big.MouseLeave += TreeViewItem_MouseLeave;
            big.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);

            big.ContextMenu = new ContextMenu();
            big.ContextMenu.Opened += ContextMenu_Opened;
            big.ContextMenu.Closed += ContextMenu_Closed;
            MenuItem menuItem1 = new MenuItem { Header = "Create File", Cursor = Cursors.Hand };
            menuItem1.PreviewMouseDown += MenuItemCreate_Click;
            MenuItem menuItem2 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
            menuItem2.PreviewMouseDown += MenuItemDelete_Click;
            MenuItem menuItem3 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
            menuItem3.PreviewMouseDown += MenuItemRename_Click;
            big.ContextMenu.Items.Add(menuItem1);
            big.ContextMenu.Items.Add(menuItem2);
            big.ContextMenu.Items.Add(menuItem3);

            if (storageitems == null || storageitems.Count == 0)
            {
                big.Header = "No cloud files.";
                TreeViewCloud.IsHitTestVisible = false;
                return big;
            }

            foreach (FileObject item in storageitems)
            {
                if (item.IsFolder)
                {
                    TreeViewItem underfolder = await GetAllCloudPath($"{userpath}/{item.Name}");
                    underfolder.FontSize = 14;
                    underfolder.Tag = ($"{userpath}/{item.Name}", FileManager.FileType.Cloud);
                    underfolder.PreviewMouseDown += Fileitem_PreviewMouseDown;
                    underfolder.MouseEnter += TreeViewItem_MouseEnter;
                    underfolder.MouseLeave += TreeViewItem_MouseLeave;
                    underfolder.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    big.Items.Add(underfolder);

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Opened += ContextMenu_Opened;
                    contextMenu.Closed += ContextMenu_Closed;

                    MenuItem menuItem21 = new MenuItem { Header = "Create File", Cursor = Cursors.Hand };
                    menuItem21.PreviewMouseDown += MenuItemCreate_Click;
                    MenuItem menuItem22 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
                    menuItem22.PreviewMouseDown += MenuItemDelete_Click;
                    MenuItem menuItem23 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
                    menuItem23.PreviewMouseDown += MenuItemRename_Click;
                    contextMenu.Items.Add(menuItem21);
                    contextMenu.Items.Add(menuItem22);
                    contextMenu.Items.Add(menuItem23);
                    underfolder.ContextMenu = contextMenu;
                }
                else
                {
                    TreeViewItem small = new TreeViewItem();
                    small.Tag = ($"{userpath}/{item.Name}", FileManager.FileType.Cloud);
                    small.Header = item.Name;
                    small.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    small.Cursor = Cursors.Hand;
                    small.PreviewMouseDown += Fileitem_PreviewMouseDown;
                    small.PreviewMouseDoubleClick += Fileitem_MouseDoubleClick;
                    small.MouseEnter += TreeViewItem_MouseEnter;
                    small.MouseLeave += TreeViewItem_MouseLeave;
                    small.FontSize = 14;

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Opened += ContextMenu_Opened;
                    contextMenu.Closed += ContextMenu_Closed;

                    MenuItem menuItem21 = new MenuItem { Header = "Open", Cursor = Cursors.Hand };
                    menuItem21.PreviewMouseDown += MenuItemOpen_Click;
                    MenuItem menuItem22 = new MenuItem { Header = "Delete", Cursor = Cursors.Hand };
                    menuItem22.PreviewMouseDown += MenuItemDelete_Click;
                    MenuItem menuItem23 = new MenuItem { Header = "Rename", Cursor = Cursors.Hand };
                    menuItem23.PreviewMouseDown += MenuItemRename_Click;
                    contextMenu.Items.Add(menuItem21);
                    contextMenu.Items.Add(menuItem22);
                    contextMenu.Items.Add(menuItem23);
                    small.ContextMenu = contextMenu;

                    big.Items.Add(small);
                }
            }
            return big;
        }

        private async void Fileitem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            ButtonDel.IsEnabled = true;
            ButtonCreate.IsEnabled = true;
            foreach(System.Windows.Controls.Label label in StackPanelHistory.Children)
            {
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            }
            selectedTreeViewItem = (TreeViewItem)sender;
            (string path, FileManager.FileType type) = ((string, FileManager.FileType))selectedTreeViewItem.Tag;
            if(!Directory.Exists(path) && type == FileManager.FileType.Local)
                ButtonCreate.IsEnabled = false;
            else if(FileManager.FileType.Cloud == type)
            {
                List<FileObject>? result = await MainWindow.supabase.Storage.From("MarkIt").List(path);
                if (!result.Any())
                {
                    ButtonCreate.IsEnabled = false;
                }
            }
            if(selectedTreeViewItem != null)
                selectedTreeViewItem.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        public void Show()
        {
            ButtonDel.IsEnabled = true;
            Logger.logger.Debug("Open file-workspace.");
            Update();
            timerUpdate.Start();
            Canvas.SetLeft(BorderMain, 10);
            if (MainWindow.GeneralSettings.iconAnimations)
                timerBlurIn.Start();
            else
                RectBackground.Opacity = 0.5;
            RectBackground.IsHitTestVisible = true;
            // Valentin-GPT
            // prompt: wie hast du das gemacht bei raumwechsel
            DoubleAnimation animation = new DoubleAnimation
            {
                To = 13, // Zielposition
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
            BorderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            // Ende
        }

        public void Hide()
        {
            selectedFileHistoryItem = null;
            selectedTreeViewItem = null;
            ButtonCreate.IsEnabled = true;
            timerUpdate.Stop();
            if (MainWindow.GeneralSettings.iconAnimations)
                timerBlurOut.Start();
            else
                RectBackground.Opacity = 0;
            RectBackground.IsHitTestVisible = false;
            // Valentin-GPT
            // prompt: wie hast du das gemacht bei raumwechsel
            DoubleAnimation animation = new DoubleAnimation
            {
                To = -BorderMain.Width - 20, // Zielposition
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
            BorderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            // Ende
        }

        private void TimerBlurIn_Tick(object? sender, EventArgs e)
        {
            RectBackground.Opacity += 0.05;
            if(RectBackground.Opacity >= 0.5)
            {
                timerBlurIn.Stop();
            }
        }

        private void TimerBlurOut_Tick(object? sender, EventArgs e)
        {
            RectBackground.Opacity -= 0.05;
            if (RectBackground.Opacity <= 0)
            {
                timerBlurOut.Stop();
            }
        }

        private void Fileitem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            selectedTreeViewItem = (TreeViewItem)sender;
            ButtonOpen_Click(sender, e);
            Hide();
        }

        private void ScrollViewerMain_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // CHATGPT Anfang
            // Prompt: Wieso kann ich nicht scrollen, wenn ich ueber einem treeview bin wpf
            ScrollViewerMain.RaiseEvent(
                new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent
                });

            e.Handled = true;
            // CHATGPT Ende
        }

        private void BorderMain_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = (DependencyObject)e.OriginalSource;

            if (source == BorderMain || source == TreeViewLocal || source == TreeViewCloud)
            {
                selectedFileHistoryItem = null;
                selectedTreeViewItem = null;
                foreach (System.Windows.Controls.Label label1 in StackPanelHistory.Children)
                {
                    label1.Background = Brushes.Transparent;
                }
                ButtonDel.IsEnabled = true;
                ButtonCreate.IsEnabled = true;
                MessageBox.Show("Click");
            }
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (selectedTreeViewItem == null)
            {
                CreateFromNothingSelected();
                return;
            }
            WindowMessageBox box;
            (string path, FileManager.FileType type) = ((string, FileManager.FileType))selectedTreeViewItem.Tag;
            if(type == FileManager.FileType.Local || (type == FileManager.FileType.Root && System.IO.Path.IsPathRooted(path)))
            {
                if (File.Exists(path))
                {
                    box = new WindowMessageBox("This is a file.", "You cannot create a file in a file.");
                    box.ShowDialog();
                    return;
                }
                else if(Directory.Exists(path))
                {
                    string newPath = $"{path}/NewMarkItFile";
                    while (File.Exists($"{newPath}.md"))
                        newPath += "-Copy";
                    File.WriteAllText($"{newPath}.md", "");
                    UpdateFileTreeLocal();
                    return;
                }
                else
                {
                    box = new WindowMessageBox("This is not a folder.", "You have to select a folder to create a item.");
                    box.ShowDialog();
                    return;
                }
            }
            else if(type == FileManager.FileType.Cloud ||(type == FileManager.FileType.Root && !System.IO.Path.IsPathRooted(path)))
            {
                string newPath = $"{path}/NewMarkItFile";
                while (true)
                {
                    try
                    {
                        await MainWindow.supabase.Storage.From("MarkIt").Download($"{newPath}.md", null);
                        newPath += "-Copy";
                        continue;
                    }
                    catch
                    {
                        break;
                    }
                }
                await MainWindow.FileManager.Upload($"{newPath}.md", "", MainWindow.loadingScreen);
                UpdateFileTreeCloud();
            }
        }

        private async void CreateFromNothingSelected()
        {
            WindowCreateAssistent window = new WindowCreateAssistent();
            window.ShowDialog();
            await UpdateFileTreeCloud();
            UpdateFileTreeLocal();
        }
    }
}
