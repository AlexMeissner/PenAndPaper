class Sound {
    audio: HTMLAudioElement;

    public constructor(filePath: string, isLooped: boolean) {
        console.log("Sound: " + filePath);
        this.audio = new Audio(filePath);
        this.audio.preload = "auto";
        this.audio.loop = isLooped;

        this.audio.addEventListener('error', (event) => {
            const audio = event.currentTarget as HTMLAudioElement;

            if (audio.error) {
                console.error('Audio playback error details:');
                console.error(`Code: ${audio.error.code}`);
                console.error(`Message: ${audio.error.message || 'No message available'}`);
            } else {
                console.error('Audio playback error occurred but no additional details are available.');
            }
        });
    }

    public async fadeIn(duration: number, volume: number): Promise<void> {
        this.setVolume(0);
        await this.play();
        // todo
    }

    public fadeOut(duration: number): void {
        // todo
        this.audio = null;
    }

    public async play(): Promise<void> {
        await this.audio.play();
    }

    public setVolume(volume: number): void {
        this.audio.volume = volume;
    }
}

function createSound(filePath: string, isLooped: boolean): Sound {
    return new Sound(filePath, isLooped);
}
