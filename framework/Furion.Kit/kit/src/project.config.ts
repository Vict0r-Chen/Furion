interface ProjectConfig {
  serverAddress: string;
}

const projectConfig = (window as any).projectConfig as ProjectConfig;

export default projectConfig;
