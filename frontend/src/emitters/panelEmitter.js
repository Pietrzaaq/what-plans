import mitt from 'mitt';

export const panelEmitter = new mitt();

export const PANEL_EVENTS = {
    OPEN: 'OPEN',
    CLOSE: 'CLOSE',
    RELOAD: 'RELOAD',
};