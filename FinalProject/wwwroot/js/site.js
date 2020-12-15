// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let audioFile = document.getElementById('player');

console.log('Current State');
console.log(`Network State: ${audioFile.networkState}`);
console.log(`Buffered: ${audioFile.duration}`);