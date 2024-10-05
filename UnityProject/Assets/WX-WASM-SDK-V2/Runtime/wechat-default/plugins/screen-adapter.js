wx.onWindowResize((res) => {
    window.innerWidth = res.windowWidth;
    window.innerHeight = res.windowHeight;
});
wx.onDeviceOrientationChange(() => {
    const info = wx.getWindowInfo ? wx.getWindowInfo() : wx.getSystemInfoSync();
    window.innerWidth = info.screenWidth;
    window.innerHeight = info.screenHeight;
});
