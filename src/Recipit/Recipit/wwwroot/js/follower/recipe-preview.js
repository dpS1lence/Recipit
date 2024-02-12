﻿function generatePreview(imgUrl, username) {
    console.log('--' + imgUrl);
    console.log('--' + username);
    var displayPreview = document.getElementById('display-preview');
    var btnPreview = document.getElementById('btn-preview');

    var imageUrl = '';
    var titleData = '';
    var descriptionData = '';

    document.getElementById('image').addEventListener('change', function (event) {
        var file = event.target.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                imageUrl = e.target.result;
            };
            reader.readAsDataURL(file);
            updatePreview();
        }
    });

    document.getElementById('postTitle').addEventListener('change', function (event) {
        titleData = event.target.value;
    });

    document.getElementById('postDescription').addEventListener('change', function (event) {
        descriptionData = event.target.value;
    });

    displayPreview.addEventListener('click', function () {
        updatePreview();
    });

    btnPreview.addEventListener('click', function () {
        updatePreview();
    });

    function updatePreview() {
        let cont = document.getElementById('dynamic-content');
        let previewHtml = `
                                                        <div class="post-container">
                                                       <header class="post-header">
                                                           <div class="post-header-info">
                                                               <div class="post-header-image">
                                                                   <img class="header-img-data" src="${imgUrl}" />
                                                                       <h1 class="header-img-text">${username}</h1>
                                                               </div>
                                                               <div class="post-header-hour">
                                                                   <h1><strong>‧</strong></h1>
                                                               </div>
                                                           </div>
                                                           <div class="post-header-buttons">
                                                               <a class="follow-btn">1 minute ago</a>
                                                           </div>
                                                       </header>
                                                       <div class="post-content">
                                                           <div class="post-content-bg"></div>
                                                               <h1 class="content-title">${titleData}</h1>
                                                                   <img class="uploaded-image-file" src="${imageUrl}" alt="Uploaded Image" />
                                                           <p class="content-text-container">
                                                                   ${descriptionData}
                                                           </p>
                                                       </div>
                                                       <div class="post-footer">
                                                           <div class="footer-buttons">
                                                               <a class="footer-btn-action" href="#"><i class="fa fa-heart"></i> 0</a>
                                                               <a class="footer-btn-action" href="#"><i class="fa fa-comment"></i> 0</a>
                                                               <a class="footer-btn-action" href="#"><i class="fa fa-share"></i> Share</a>
                                                           </div>
                                                       </div>
                                                   </div>
                                                        `;

        cont.innerHTML = previewHtml;
    }
}