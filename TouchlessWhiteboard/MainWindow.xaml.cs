using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition;
using Microsoft.UI.Text;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TouchlessWhiteboard.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using WinUIEx;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Microsoft.UI.Input.DragDrop;
using Windows.Security.Authentication.OnlineId;
using Windows.ApplicationModel.Core;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI;
using Windows.UI;
using Microsoft.UI.Input;

namespace TouchlessWhiteboard;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private bool IsToolBarOpen = true;
    private int screenHeight;
    private int screenWidth;


    private Point MouseDownLocation;
    public MainWindow()
    {
        ViewModel = Ioc.Default.GetService<MainWindowViewModel>();
        SettingsWindowViewModel settingsWindowViewModel = Ioc.Default.GetService<SettingsWindowViewModel>();

        //ViewModel.IsTouchlessArtsEnabled = settingsWindowViewModel.IsTouchlessArtsEnabled;
        //ViewModel.IsEraserEnabled = settingsWindowViewModel.IsEraserEnabled;
        //ViewModel.IsShapesEnabled = settingsWindowViewModel.IsShapesEnabled;
        //ViewModel.IsSelectionEnabled = settingsWindowViewModel.IsSelectionEnabled;
        //ViewModel.IsStickyNotesEnabled = settingsWindowViewModel.IsStickyNotesEnabled;
        //ViewModel.IsCameraEnabled = settingsWindowViewModel.IsCameraEnabled;
        //ViewModel.IsSearchEnabled = settingsWindowViewModel.IsSearchEnabled;
        //ViewModel.IsCopilotEnabled = settingsWindowViewModel.IsCopilotEnabled;
        //ViewModel.IsToolsEnabled = settingsWindowViewModel.IsToolsEnabled;
        //ViewModel.IsInAir3DMouseEnabled = settingsWindowViewModel.IsInAir3DMouseEnabled;
        //ViewModel.IsCalculatorEnabled = settingsWindowViewModel.IsCalculatorEnabled;
        //ViewModel.IsRulerEnabled = settingsWindowViewModel.IsRulerEnabled;
        //ViewModel.IsTimerEnabled = settingsWindowViewModel.IsTimerEnabled;
        //ViewModel.IsAlarmEnabled = settingsWindowViewModel.IsAlarmEnabled;
        //ViewModel.IsQuickFileAccessEnabled = settingsWindowViewModel.IsQuickFileAccessEnabled;

        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Visible;
        ViewModel.IsIconShown = Visibility.Collapsed;
        ViewModel.IsTouchlessArtsOpen = Visibility.Collapsed;
        this.InitializeComponent();
        this.SetIsAlwaysOnTop(true);
        this.Maximize();
        CreateButtons(ToolBarPanel);
        //remove title bar
        //var coreTitleBar = this.GetAppWindow().TitleBar;
        //coreTitleBar.ExtendsContentIntoTitleBar = true;

        screenHeight = this.AppWindow.Size.Height;
        screenWidth = this.AppWindow.Size.Width;
    }
    public MainWindowViewModel? ViewModel { get; }
    public SettingsWindowViewModel? SettingsWindowViewModel { get; }

    private TranslateTransform dragtranslation;

    private void objectManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        if (sender.GetType().Name != "Line")
        {
            var draggedObject = e.OriginalSource as UIElement;
            if (sender.GetType().Name == "Ellipse")
            {
                if (!isDragging) return;
                draggedObject = e.OriginalSource as Ellipse;
            }
            else if (sender.GetType().Name == "Rectangle")
            {
                if (!isDragging) return;
                draggedObject = e.OriginalSource as Rectangle;
            }
            else if (sender.GetType().Name == "Polygon")
            {
                if (!isDragging) return;
                draggedObject = e.OriginalSource as Polygon;
            }
            else if (sender.GetType().Name == "StackPanel")
            {
                draggedObject = e.OriginalSource as StackPanel;
            }
            (draggedObject.RenderTransform as TranslateTransform).X += e.Delta.Translation.X;
            (draggedObject.RenderTransform as TranslateTransform).Y += e.Delta.Translation.Y;
        }
        else if (sender.GetType().Name == "Line")
        {
            if (!isDragging) return;
            var draggedObject = e.OriginalSource as Line;

            // Calculate the new position for both ends of the line
            var x1 = draggedObject.X1 + e.Delta.Translation.X;
            var y1 = draggedObject.Y1 + e.Delta.Translation.Y;
            var x2 = draggedObject.X2 + e.Delta.Translation.X;
            var y2 = draggedObject.Y2 + e.Delta.Translation.Y;

            // Update the position of both ends of the line
            draggedObject.X1 = x1;
            draggedObject.Y1 = y1;
            draggedObject.X2 = x2;
            draggedObject.Y2 = y2;
        }
    }

    public void CreateButtons(StackPanel panel)
    {
        panel.Children.Clear();
        if (IsToolBarOpen)
        {
            if (ViewModel.IsTouchlessArtsEnabled)
                panel.Children.Add(CreateButton("Touchless Arts"));
            if (ViewModel.IsEraserEnabled)
                panel.Children.Add(CreateButton("Eraser"));
            if (ViewModel.IsShapesEnabled)
                panel.Children.Add(CreateButton("Shapes"));
            if (ViewModel.IsSelectionEnabled)
                panel.Children.Add(CreateButton("Selection"));
            if (ViewModel.IsStickyNotesEnabled)
                panel.Children.Add(CreateButton("Sticky Notes"));
            if (ViewModel.IsCameraEnabled)
                panel.Children.Add(CreateButton("Camera"));
            if (ViewModel.IsSearchEnabled)
                panel.Children.Add(CreateButton("Search"));
            if (ViewModel.IsCopilotEnabled)
                panel.Children.Add(CreateButton("Copilot"));
            if (ViewModel.IsToolsEnabled)
                panel.Children.Add(CreateButton("Tools"));
            if (ViewModel.IsInAir3DMouseEnabled)
                panel.Children.Add(CreateButton("In Air 3D Mouse"));
            panel.Children.Add(CreateButton("Close"));
        }
        else
        {
            panel.Children.Add(CreateButton("Open"));
        }
    }

    private Button CreateButton(string buttonType)
    {

        Button button = new Button();
        Style toolBarButtonStyle = (Style)Application.Current.Resources["ToolBarButtonStyle"];
        button.Style = toolBarButtonStyle;

        // Create an Image control
        Image image = new Image();
        Style toolBarButtonImageStyle = (Style)Application.Current.Resources["ToolBarButtonImageStyle"];
        image.Style = toolBarButtonImageStyle;

        // Set the source of the Image control based on the buttonType
        switch (buttonType)
        {
            case "Touchless Arts":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Touchless-Arts-icon.png"));
                button.Click += TouchlessArts_Clicked;
                break;
            case "Eraser":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Eraser-icon.png"));
                break;
            case "Shapes":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Shapes-icon.png"));
                break;
            case "Selection":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Selection-icon.png"));
                break;
            case "Sticky Notes":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Sticky-Notes-icon.png"));
                break;
            case "Camera":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Camera-icon.png"));
                button.Click += Camera_Clicked;
                break;
            case "Search":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Search-icon.png"));
                button.Click += Search_Clicked;
                break;
            case "Copilot":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Copilot-icon.png"));
                button.Click += Copilot_Clicked;
                break;
            case "Tools":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Tools-icon.png"));
                break;
            case "In Air 3D Mouse":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/In-Air-3D-Mouse-icon.png"));
                break;
            case "Close":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Close-icon.png"));
                button.Click += CloseToolBar_Clicked;
                break;
            case "Open":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Open-icon.png"));
                button.Click += OpenToolBar_Clicked;
                break;
        }

        // Set the Image control as the content of the button
        button.Content = image;

        // Set other properties and event handlers as needed
        return button;

    }
    private void MinimizeTouchlessWhiteboard()
    {
        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Collapsed;
        ViewModel.IsIconShown = Visibility.Visible;
        this.MoveAndResize(10, 10, 150, 150);
        var coreTitleBar = this.GetAppWindow().TitleBar;
        coreTitleBar.ExtendsContentIntoTitleBar = true;
        this.SetIsMaximizable(false);
        this.Bindings.Update();
    }

    private void TouchlessArts_Clicked(object sender, RoutedEventArgs e)
    {
        ViewModel.IsTouchlessArtsOpen = Visibility.Visible;
        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Collapsed;
        ViewModel.IsIconShown = Visibility.Collapsed;
        this.Bindings.Update();
    }
    private void Camera_Clicked(object sender, RoutedEventArgs e)
    {
        // open snipping tool
        Windows.System.Launcher.LaunchUriAsync(new Uri("ms-screenclip:"));
    }

    private void Search_Clicked(object sender, RoutedEventArgs e)
    {
        // open bing search
        Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.bing.com/"));
        MinimizeTouchlessWhiteboard();
    }

    private void Copilot_Clicked(object sender, RoutedEventArgs e)
    {
        Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.bing.com/chat"));
        MinimizeTouchlessWhiteboard();
    }

    private void CloseToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsToolBarOpen = false;
        CreateButtons(ToolBarPanel);
    }

    private void OpenToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsToolBarOpen = true;
        CreateButtons(ToolBarPanel);
    }

    private void TouchlessWhiteboard_MouseDown(object sender, PointerRoutedEventArgs e)
    {
        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Visible;
        ViewModel.IsIconShown = Visibility.Collapsed;
        // remove title bar
        var coreTitleBar = this.GetAppWindow().TitleBar;
        coreTitleBar.ExtendsContentIntoTitleBar = true;
        this.Bindings.Update();
        this.SetWindowSize(screenWidth, screenHeight);
        this.SetIsMaximizable(true);
    }

    private Brush currentBrush = new SolidColorBrush(Colors.Black);
    private bool isDrawing = false;
    private bool isErasing = false;
    private bool isDragging = false;
    private Point startPoint;
    private Stack<UIElement> elementsList = new Stack<UIElement>();
    private Stack<int> elementNumStack = new Stack<int>();
    private Stack<UIElement> removedElementsList = new Stack<UIElement>();
    private Stack<int> removedElementNumStack = new Stack<int>();
    private int lastSize = 0;
    private bool isLine = false;

    private int SelectedIndex = 0;
    private Ellipse currentEllipse;
    private Rectangle currentRectangle;
    private Polygon currentPolygon;

    private enum DrawingMode
    {
        Circle,
        Rectangle,
        Triangle
    }

    private DrawingMode currentDrawingMode = DrawingMode.Rectangle;

    public List<double> BrushThickness { get; } = new List<double>
    {
        4,
        8,
        18,
        20,
        31,
        42,
        54,
        66,
        78,
        80,
        94,
        108,
        116,
        148,
        172
    };

    private async void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (ViewModel.IsTouchlessArtsOpen == Visibility.Collapsed) return;
        if (isDragging) return;
        if (e.Pointer.PointerDeviceType.Equals(PointerDeviceType.Mouse))
        {
            //if (colorPickerButton.IsChecked == true)
            //{
            //    var pointerPosition = e.GetCurrentPoint(Whiteboard);
            //    int x = (int)pointerPosition.Position.X;
            //    int y = (int)pointerPosition.Position.Y;
            //    RenderTargetBitmap renderBitmap = new RenderTargetBitmap();
            //    await renderBitmap.RenderAsync(Whiteboard);
            //    var pixelBuffer = await renderBitmap.GetPixelsAsync();
            //    var pixelData = pixelBuffer.ToArray();
            //    int pixelIndex = (y * renderBitmap.PixelWidth + x) * 4;
            //    byte[] pixelColor = new byte[4];
            //    Array.Copy(pixelData, pixelIndex, pixelColor, 0, 4);
            //    Color color = Color.FromArgb(pixelColor[3], pixelColor[2], pixelColor[1], pixelColor[0]);
            //    CurrentColor.Background = currentBrush = new SolidColorBrush(color);
            //    return;
            //}
            isDrawing = true;
            startPoint = e.GetCurrentPoint(Whiteboard).Position;
            if (shapesButton.IsChecked == true)
            {
                switch (currentDrawingMode)
                {
                    case DrawingMode.Circle:
                        currentEllipse = new Ellipse
                        {
                            Width = 0,
                            Height = 0,
                            Stroke = currentBrush,
                            StrokeThickness = BrushThickness[SelectedIndex]
                        };
                        currentEllipse.ManipulationDelta += objectManipulationDelta;
                        currentEllipse.ManipulationMode = ManipulationModes.All;
                        currentEllipse.RenderTransform = new TranslateTransform();
                        Whiteboard.Children.Add(currentEllipse);
                        Canvas.SetLeft(currentEllipse, startPoint.X);
                        Canvas.SetTop(currentEllipse, startPoint.Y);
                        break;
                    case DrawingMode.Rectangle:
                        currentRectangle = new Rectangle {
                            Width = 0,
                            Height = 0,
                            Stroke = currentBrush,
                            StrokeThickness = BrushThickness[SelectedIndex]
                        };
                        currentRectangle.ManipulationDelta += objectManipulationDelta;
                        currentRectangle.ManipulationMode = ManipulationModes.All;
                        currentRectangle.RenderTransform = new TranslateTransform();
                        Whiteboard.Children.Add(currentRectangle);
                        Canvas.SetLeft(currentRectangle, startPoint.X);
                        Canvas.SetTop(currentRectangle, startPoint.Y);
                        break;
                    case DrawingMode.Triangle:
                        currentPolygon = new Polygon
                        {
                            Stroke = currentBrush,
                            StrokeThickness = BrushThickness[SelectedIndex]
                        };
                        currentPolygon.Points.Add(startPoint);
                        currentPolygon.Points.Add(startPoint);
                        currentPolygon.Points.Add(startPoint);
                        currentPolygon.ManipulationDelta += objectManipulationDelta;
                        currentPolygon.ManipulationMode = ManipulationModes.All;
                        currentPolygon.RenderTransform = new TranslateTransform();
                        Whiteboard.Children.Add(currentPolygon);
                        break;
                }
            }
        }
    }

    private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (ViewModel.IsTouchlessArtsOpen == Visibility.Collapsed) return;
        if (!isDrawing) return;
        if (isDragging) return;
        if (shapesButton.IsChecked == true)
        {
            Point currentPoint = e.GetCurrentPoint(Whiteboard).Position;
            switch (currentDrawingMode)
            {
                case DrawingMode.Circle:
                    double newWidth = Math.Abs(currentPoint.X - startPoint.X) * 2;
                    double newHeight = Math.Abs(currentPoint.Y - startPoint.Y) * 2;
                    currentEllipse.Width = newWidth;
                    currentEllipse.Height = newHeight;
                    double left = Math.Min(startPoint.X, currentPoint.X);
                    double top = Math.Min(startPoint.Y, currentPoint.Y);
                    Canvas.SetLeft(currentEllipse, left);
                    Canvas.SetTop(currentEllipse, top);
                    elementsList.Push(currentEllipse);
                    break;
                case DrawingMode.Rectangle:
                    newWidth = Math.Abs(currentPoint.X - startPoint.X);
                    newHeight = Math.Abs(currentPoint.Y - startPoint.Y);
                    currentRectangle.Width = newWidth;
                    currentRectangle.Height = newHeight;

                    left = Math.Min(startPoint.X, currentPoint.X);
                    top = Math.Min(startPoint.Y, currentPoint.Y);
                    Canvas.SetLeft(currentRectangle, left);
                    Canvas.SetTop(currentRectangle, top);
                    elementsList.Push(currentRectangle);
                    break;
                case DrawingMode.Triangle:
                    double centerX = (startPoint.X + currentPoint.X) / 2;
                    double centerY = (startPoint.Y + currentPoint.Y) / 2;
                    double sideLength = Math.Min(Math.Abs(currentPoint.X - startPoint.X), Math.Abs(currentPoint.Y - startPoint.Y));
                    Point vertex1 = new Point(centerX, centerY - (sideLength / 2));
                    Point vertex2 = new Point(centerX - (sideLength / 2), centerY + (sideLength / 2));
                    Point vertex3 = new Point(centerX + (sideLength / 2), centerY + (sideLength / 2));
                    currentPolygon.Points.Clear();
                    currentPolygon.Points.Add(vertex1);
                    currentPolygon.Points.Add(vertex2);
                    currentPolygon.Points.Add(vertex3);
                    currentPolygon.Points.Add(vertex1);
                    elementsList.Push(currentPolygon);
                    break;
            }
        }

        else if (brushButton.IsChecked == true)
        {
            Line line = new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = e.GetCurrentPoint(Whiteboard).Position.X,
                Y2 = e.GetCurrentPoint(Whiteboard).Position.Y,
                Stroke = currentBrush,
                StrokeThickness = BrushThickness[SelectedIndex],
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round
            };
            Line lineBlur = new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = e.GetCurrentPoint(Whiteboard).Position.X,
                Y2 = e.GetCurrentPoint(Whiteboard).Position.Y,
                Stroke = currentBrush,
                StrokeThickness = BrushThickness[SelectedIndex] + 10,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                Opacity = 0.2
            };
            line.ManipulationDelta += objectManipulationDelta;
            line.ManipulationMode = ManipulationModes.All;
            line.RenderTransform = new TranslateTransform();
            Whiteboard.Children.Add(lineBlur);
            Whiteboard.Children.Add(line);
            startPoint = e.GetCurrentPoint(Whiteboard).Position;
            elementsList.Push(lineBlur);
            elementsList.Push(line);
        }
        else
        {
            Line line = new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = e.GetCurrentPoint(Whiteboard).Position.X,
                Y2 = e.GetCurrentPoint(Whiteboard).Position.Y,
                Stroke = currentBrush,
                StrokeThickness = 1,
            };
            line.ManipulationDelta += objectManipulationDelta;
            line.ManipulationMode = ManipulationModes.All;
            line.RenderTransform = new TranslateTransform();
            Whiteboard.Children.Add(line);
            startPoint = e.GetCurrentPoint(Whiteboard).Position;
            elementsList.Push(line);
        }
    }

    private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        isDrawing = false;
        isErasing = false;
        elementNumStack.Push(elementsList.Count - lastSize);
        lastSize = elementsList.Count;
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        Whiteboard.Children.Clear();
    }
    private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        currentBrush = new SolidColorBrush(args.NewColor);
        CurrentColor.Background = new SolidColorBrush(args.NewColor);
    }

    private void CoboBoxBrushThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = CoboBoxBrushThickness.SelectedIndex;
    }
    private void CursorButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleButton clickedButton = sender as ToggleButton;
        brushButton.IsChecked = false;
        pencilButton.IsChecked = false;
        eraserButton.IsChecked = false;
        //colorPickerButton.IsChecked = false;
        shapesButton.IsChecked = false;
        cursorButton.IsChecked = true;
        isDragging = true;
    }
    
    private void PencilButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleButton clickedButton = sender as ToggleButton;
        cursorButton.IsChecked = false;
        brushButton.IsChecked = false;
        eraserButton.IsChecked = false;
        //colorPickerButton.IsChecked = false;
        shapesButton.IsChecked = false;
        pencilButton.IsChecked = true;
        isDragging = false;
    }

    private void BrushButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleButton clickedButton = sender as ToggleButton;
        cursorButton.IsChecked = false;
        pencilButton.IsChecked = false;
        eraserButton.IsChecked = false;
        //colorPickerButton.IsChecked = false;
        shapesButton.IsChecked = false;
        brushButton.IsChecked = true;
        isDragging = false;
    }

    private void EraserButton_Click(object sender, RoutedEventArgs e)
    {
        isErasing = !isErasing;
        //currentBrush = new SolidColorBrush(Colors.Transparent);
        cursorButton.IsChecked = false;
        brushButton.IsChecked = false;
        pencilButton.IsChecked = false;
        //colorPickerButton.IsChecked = false;
        shapesButton.IsChecked = false;
        eraserButton.IsChecked = true;
        isDragging = false;
    }

    private void CoboBoxShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (CoboBoxShapes.SelectedIndex)
        {
            case 0:
                currentDrawingMode = DrawingMode.Circle;
                break;
            case 1:
                currentDrawingMode = DrawingMode.Rectangle;
                break;
            case 2:
                currentDrawingMode = DrawingMode.Triangle;
                break;
        }
    }

    //private void PickerButton_Click(object sender, RoutedEventArgs e)
    //{
    //    cursorButton.IsChecked = false;
    //    brushButton.IsChecked = false;
    //    pencilButton.IsChecked = false;
    //    eraserButton.IsChecked = false;
    //    shapesButton.IsChecked = false;
    //    colorPickerButton.IsChecked = true;
    //    isDragging = false;
    //}

    private void ShapesButton_Click(object sender, RoutedEventArgs e)
    {
        cursorButton.IsChecked = false;
        brushButton.IsChecked = false;
        pencilButton.IsChecked = false;
        eraserButton.IsChecked = false;
        //colorPickerButton.IsChecked = false;
        shapesButton.IsChecked = true;
        isDragging = false;
    }

    private void UndoButton_Click(object sender, RoutedEventArgs e)
    {
        if (elementsList.Count > 0)
        {
            // if elementslist is not empty
            if (elementsList.Count > 0)
            {
                int numLines = elementNumStack.Pop();
                for (int i = 0; i < numLines; i++)
                {
                    UIElement removed = elementsList.Pop();
                    Whiteboard.Children.Remove(removed);
                    removedElementsList.Push(removed);
                }
                removedElementNumStack.Push(numLines);
                lastSize = lastSize - numLines;
            }
        }
    }

    private void RedoButton_Click(object sender, RoutedEventArgs e)
    {
        if (removedElementsList.Count > 0)
        {
            int numLines = removedElementNumStack.Pop();
            if (removedElementsList.First().GetType().Name == "Line")
            {
                for (int i = 0; i < numLines; i++)
                {
                    UIElement added = removedElementsList.Pop();
                    Whiteboard.Children.Add(added);
                    elementsList.Push(added);
                }
                elementNumStack.Push(numLines);
                lastSize = lastSize + numLines;
            }
            else
            {
                UIElement added = removedElementsList.Pop();
                Whiteboard.Children.Add(added);
                elementsList.Push(added);
                for (int i = 0; i < numLines - 1; i++)
                {
                    removedElementsList.Pop();
                }
                elementNumStack.Push(1);
                lastSize = lastSize + 1;
            }
            
        }
    }
}
