var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Sound {
    constructor(filePath, isLooped) {
        console.log("Sound: " + filePath);
        this.audio = new Audio(filePath);
        this.audio.preload = "auto";
        this.audio.loop = isLooped;
        this.audio.addEventListener('error', (event) => {
            const audio = event.currentTarget;
            if (audio.error) {
                console.error('Audio playback error details:');
                console.error(`Code: ${audio.error.code}`);
                console.error(`Message: ${audio.error.message || 'No message available'}`);
            }
            else {
                console.error('Audio playback error occurred but no additional details are available.');
            }
        });
    }
    fadeIn(duration, volume) {
        return __awaiter(this, void 0, void 0, function* () {
            this.setVolume(0);
            yield this.play();
            // todo
        });
    }
    fadeOut(duration) {
        // todo
        this.audio = null;
    }
    play() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.audio.play();
        });
    }
    setVolume(volume) {
        this.audio.volume = volume;
    }
}
function createSound(filePath, isLooped) {
    return new Sound(filePath, isLooped);
}
