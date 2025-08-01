window.checkNotificationExists = function () {
    const notification = document.querySelector('.rz-notification');
    console.log('Notification check result:', notification ? true : false);
    return notification ? true : false;
};