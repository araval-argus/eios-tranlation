export interface FilterItem {
    key: string;
    label: string;
    labelTraslated?: string;
    count: number;
    selected: boolean;
    visible: boolean;
    country?: string;
    property?: string;
    extraInfo?: ExtraInfo;
}
export interface ExtraInfo {
    level?: number;
    areaType?: any;
    iso?: any;
    subAreaCodes?: string[];
    label: string;
    icon: string;
}
