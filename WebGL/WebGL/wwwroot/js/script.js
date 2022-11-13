var gl;
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
}

function bufferData(type, size, dataptr, usage) {
    let arr = new Float32Array(Module.HEAPF32.buffer, dataptr, size/4)
    gl.bufferData(type, arr, usage);
}

function run(dt) {
    App?.WebGL?.Pages.Index.Update(dt)
    requestAnimationFrame(run);
}


function debugprint(data){
    console.log(data);
}