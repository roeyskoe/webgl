﻿var gl;
var prog;
var App;
async function init() {
    let canvas = document.getElementById("canvas");

    console.log(canvas);

    gl = canvas.getContext("webgl2");
    gl.canvas.width = visualViewport.width;
    gl.canvas.height = visualViewport.height;

    gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    App = await getAssemblyExports("WebGL.dll");

    canvas.addEventListener("mousemove", (e) => App?.WebGL?.Pages.Index.MouseMove(e.clientX, e.clientY));
    canvas.addEventListener("mousedown", (e) => App?.WebGL?.Pages.Index.MouseDown(e.clientX, e.clientY));
    canvas.addEventListener("mouseup", (e) => App?.WebGL?.Pages.Index.MouseUp(e.clientX, e.clientY));
    canvas.addEventListener("keydown", (e) => App?.WebGL?.Pages.Index.KeyDown(e.key));
    canvas.addEventListener("keyup", (e) => App?.WebGL?.Pages.Index.KeyUp(e.key));

function bufferData(type, size, dataptr, usage) {
    let arr = new Float32Array(Module.HEAPF32.buffer, dataptr, size/4)
    gl.bufferData(type, arr, usage);
}

function uniformMatrix2fv(location, dataptr) {
    let arr = new Float32Array(Module.HEAPF32.buffer, dataptr, 4)
    gl.uniformMatrix2fv(location, false, arr);
}

function uniformMatrix3fv(location, dataptr) {
    let arr = new Float32Array(Module.HEAPF32.buffer, dataptr, 9)
    gl.uniformMatrix3fv(location, false, arr);
}

function uniformMatrix4fv(location, dataptr) {
    let arr = new Float32Array(Module.HEAPF32.buffer, dataptr, 16)
    gl.uniformMatrix4fv(location, false, arr);
}

function run(dt) {

    gl.canvas.width = visualViewport.width;
    gl.canvas.height = visualViewport.height;

    gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);

    App?.WebGL?.Pages.Index.Update(dt, gl.canvas.width, gl.canvas.height)
    requestAnimationFrame(run);
}


function debugprint(_handle, name) {
    let x = gl.getUniformLocation(_handle, name);
    console.log(x);
}