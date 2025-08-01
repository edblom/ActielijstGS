window.blazorTest = function () {
    console.log("blazorTest called from JS");
    return "Hello from JavaScript!";
};

window.checkDialogExists = function () {
    const dialog = document.querySelector('.rz-dialog');
    console.log('Dialog check result:', dialog ? true : false);
    return dialog ? true : false;
};

window.checkNotificationExists = function () {
    const notification = document.querySelector('.rz-notification');
    console.log('Notification check result:', notification ? true : false);
    return notification ? true : false;
};